import { Component, OnInit, ElementRef, AfterViewInit, OnDestroy, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { DrillDataService } from '../csv-upload/csv-upload.component';
import { DrillHoleService, DrillHole } from '../../../core/services/drill-hole.service';
import * as THREE from 'three';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js';

@Component({
  selector: 'app-drill-visualization',
  imports: [CommonModule],
  templateUrl: './drill-visualization.component.html',
  styleUrl: './drill-visualization.component.scss'
})
export class DrillVisualizationComponent implements OnInit, AfterViewInit, OnDestroy {
  drillData: DrillHole[] = [];
  isDataLoaded: boolean = false;
  
  // Three.js properties
  private scene!: THREE.Scene;
  private camera!: THREE.PerspectiveCamera;
  private renderer!: THREE.WebGLRenderer;
  private controls!: OrbitControls;
  private drillObjects: THREE.Object3D[] = [];
  private labelObjects: THREE.Sprite[] = [];
  private animationFrameId?: number;

  // View options
  showLabels = true;
  showDetailedLabels = false;
  show3DView = true;

  // Route context
  projectId: number = 0;
  siteId: number = 0;

  // UI state
  isSaving = false;
  isDeleting = false;
  saveMessage: string | null = null;
  deleteMessage: string | null = null;
  errorMessage: string | null = null;

  constructor(
    private el: ElementRef, 
    private drillDataService: DrillDataService,
    private drillHoleService: DrillHoleService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    console.log('DrillVisualizationComponent initialized successfully!');
    console.log('Current URL:', this.router.url);
    this.extractRouteContext();
    this.loadDrillData();
  }

  private extractRouteContext(): void {
    console.log('=== EXTRACTING ROUTE CONTEXT ===');
    
    // Get project and site IDs from route parameters
    this.route.paramMap.subscribe(params => {
      console.log('Raw route params:', params.keys, params);
      this.projectId = +(params.get('projectId') || '0');
      this.siteId = +(params.get('siteId') || '0');
      console.log('Extracted route context from params:', { projectId: this.projectId, siteId: this.siteId });
    });

    // Fallback: try to get from query parameters (for backward compatibility)
    this.route.queryParams.subscribe(params => {
      console.log('Query params:', params);
      if (!this.projectId || !this.siteId) {
        this.projectId = this.projectId || +(params['projectId'] || '0');
        this.siteId = this.siteId || +(params['siteId'] || '0');
        console.log('Updated context from query params:', { projectId: this.projectId, siteId: this.siteId });
      }
    });
    
    console.log('Final context after extraction:', { projectId: this.projectId, siteId: this.siteId });
  }

  ngAfterViewInit(): void {
    if (this.show3DView) {
      this.initThreeJS();
    this.createScene();
    this.animate();
    
      // Load drill holes in 3D if data is available
      if (this.isDataLoaded) {
        this.create3DDrillHoles();
      }
    }
  }

  ngOnDestroy(): void {
    if (this.animationFrameId) {
      cancelAnimationFrame(this.animationFrameId);
    }
    
    // Clean up Three.js resources
    this.cleanup3D();
  }

  private loadDrillData(): void {
    this.drillData = this.drillDataService.getDrillData();
    this.isDataLoaded = this.drillData.length > 0;
    
    console.log('Loaded drill data:', this.drillData);
    console.log('Data count:', this.drillData.length);
    
    if (this.isDataLoaded) {
      console.log('Sample drill hole:', this.drillData[0]);
      this.logDrillDataSummary();
        } else {
      console.log('No drill data available. Please upload a CSV file first.');
    }
  }

  private logDrillDataSummary(): void {
    console.log('=== DRILL DATA SUMMARY ===');
    console.log(`Total holes: ${this.drillData.length}`);
    
    if (this.drillData.length > 0) {
      const sample = this.drillData[0];
      console.log('CSV Data structure:', {
        id: sample.id,
        easting: sample.easting,
        northing: sample.northing,
        elevation: sample.elevation,
        depth: sample.depth,
        azimuth: sample.azimuth,
        dip: sample.dip,
        stemming: sample.stemming
      });
    }
  }

  refreshData(): void {
    console.log('Refreshing drill data...');
    this.loadDrillData();
    
    // Refresh 3D view if enabled
    if (this.show3DView && this.scene) {
      this.clear3DDrillHoles();
      if (this.isDataLoaded) {
        this.create3DDrillHoles();
      }
    }
  }

  saveDrillData(): void {
    if (!this.isDataLoaded || this.drillData.length === 0) {
      this.errorMessage = 'No drill data to save. Please upload a CSV file first.';
      this.clearMessagesAfterDelay();
      return;
    }

    if (!this.projectId || !this.siteId) {
      this.errorMessage = 'No project/site context available. Please navigate from the sites page.';
      this.clearMessagesAfterDelay();
      return;
    }

    this.isSaving = true;
    this.errorMessage = null;
    this.saveMessage = null;

    console.log('Saving drill data to database...', {
      holes: this.drillData.length,
      projectId: this.projectId,
      siteId: this.siteId
    });

    this.drillHoleService.saveMultipleDrillHoles(this.drillData, this.projectId, this.siteId).subscribe({
      next: (savedHoles) => {
        this.isSaving = false;
        this.saveMessage = `Successfully saved ${savedHoles.length} drill holes for Site ${this.siteId} in Project ${this.projectId}!`;
        console.log('Drill holes saved successfully:', savedHoles);
        this.clearMessagesAfterDelay();
      },
      error: (error) => {
        this.isSaving = false;
        this.errorMessage = `Failed to save drill holes: ${error.message}`;
        console.error('Error saving drill holes:', error);
        this.clearMessagesAfterDelay();
      }
    });
  }

  deleteAllDrillData(): void {
    if (!this.projectId || !this.siteId) {
      this.errorMessage = 'No project/site context available. Please navigate from the sites page.';
      this.clearMessagesAfterDelay();
      return;
    }

    if (!confirm(`Are you sure you want to delete ALL drill holes for Site ${this.siteId} in Project ${this.projectId}? This action cannot be undone.`)) {
      return;
    }

    this.isDeleting = true;
    this.errorMessage = null;
    this.deleteMessage = null;

    console.log('Deleting all drill data for site from database...', {
      projectId: this.projectId,
      siteId: this.siteId
    });

    this.drillHoleService.deleteDrillHolesBySite(this.projectId, this.siteId).subscribe({
      next: () => {
        console.log('Delete operation completed, verifying site is empty...');
        
        // Verify the deletion worked by checking if site has no drill holes
        this.drillHoleService.getDrillHolesBySite(this.projectId, this.siteId).subscribe({
          next: (remainingHoles) => {
            this.isDeleting = false;
            
            if (remainingHoles.length === 0) {
              this.deleteMessage = `Successfully deleted all drill holes for Site ${this.siteId} in Project ${this.projectId}!`;
              console.log('Site verified empty - deletion successful');
            } else {
              this.deleteMessage = `Delete operation completed, but ${remainingHoles.length} holes might remain. Please check manually.`;
              console.warn('Site still contains holes after deletion:', remainingHoles.length);
            }
            
            // Clear local data
            this.drillDataService.clearDrillData();
            this.drillData = [];
            this.isDataLoaded = false;
            
            // Clear 3D visualization
            if (this.show3DView && this.scene) {
              this.clear3DDrillHoles();
            }
            
            this.clearMessagesAfterDelay();
          },
          error: (verifyError) => {
            this.isDeleting = false;
            this.deleteMessage = 'Delete operation completed but verification failed. Please reload to check.';
            console.error('Error verifying deletion:', verifyError);
            this.clearMessagesAfterDelay();
          }
        });
      },
      error: (error) => {
        this.isDeleting = false;
        this.errorMessage = `Failed to delete drill holes: ${error.message}`;
        console.error('Error deleting drill holes:', error);
        this.clearMessagesAfterDelay();
      }
    });
  }

  loadFromDatabase(): void {
    console.log('Loading drill data from database...');
    
    const loadMethod = this.projectId && this.siteId 
      ? this.drillHoleService.getDrillHolesBySite(this.projectId, this.siteId)
      : this.drillHoleService.getAllDrillHoles();
    
    loadMethod.subscribe({
      next: (holes) => {
        this.drillData = holes;
        this.isDataLoaded = holes.length > 0;
        
        // Update the shared service
        this.drillDataService.setDrillData(holes);
        
        const context = this.projectId && this.siteId 
          ? `for Site ${this.siteId} in Project ${this.projectId}`
          : 'from entire database';
        
        console.log(`Loaded drill data ${context}:`, holes.length, 'holes');
        
        // Refresh 3D view if enabled
        if (this.show3DView && this.scene) {
          this.clear3DDrillHoles();
          if (this.isDataLoaded) {
            this.create3DDrillHoles();
          }
        }
        
        if (holes.length > 0) {
          this.saveMessage = `Loaded ${holes.length} drill holes ${context}!`;
          this.clearMessagesAfterDelay();
        } else {
          this.saveMessage = `No drill holes found ${context}.`;
          this.clearMessagesAfterDelay();
        }
      },
      error: (error) => {
        this.errorMessage = `Failed to load drill holes from database: ${error.message}`;
        console.error('Error loading drill holes:', error);
        this.clearMessagesAfterDelay();
      }
    });
  }

  showDatabaseStatus(): void {
    console.log('Checking database status...');
    
    this.drillHoleService.getAllDrillHoles().subscribe({
      next: (holes) => {
        const count = holes.length;
        const ids = holes.map(h => h.id).slice(0, 10); // Show first 10 IDs
        
        console.log(`Database contains ${count} drill holes`);
        if (count > 0) {
          console.log('Drill hole IDs in database:', ids);
          if (count > 10) {
            console.log(`... and ${count - 10} more`);
          }
        }
        
        this.saveMessage = `Database status: ${count} drill holes found. ${count > 0 ? `IDs: ${ids.join(', ')}${count > 10 ? '...' : ''}` : 'Database is empty.'}`;
        this.clearMessagesAfterDelay();
      },
      error: (error) => {
        this.errorMessage = `Failed to check database status: ${error.message}`;
        console.error('Error checking database status:', error);
        this.clearMessagesAfterDelay();
      }
    });
  }

  navigateToSites(): void {
    if (this.projectId) {
      this.router.navigate(['/blasting-engineer/project-management', this.projectId, 'sites']);
    } else {
      this.router.navigate(['/blasting-engineer/project-management']);
    }
  }

  private clearMessagesAfterDelay(): void {
    setTimeout(() => {
      this.saveMessage = null;
      this.deleteMessage = null;
      this.errorMessage = null;
    }, 5000);
  }



  toggle3DView(): void {
    this.show3DView = !this.show3DView;
    
    if (this.show3DView) {
      setTimeout(() => {
        this.initThreeJS();
        this.createScene();
        this.animate();
        if (this.isDataLoaded) {
          this.create3DDrillHoles();
        }
      }, 100);
    } else {
      this.cleanup3D();
    }
  }

  toggleLabels(): void {
    this.showLabels = !this.showLabels;
    this.labelObjects.forEach(label => {
      label.visible = this.showLabels;
    });
  }

  resetCamera(): void {
    if (this.camera && this.controls) {
      this.setOptimalCameraView();
      console.log('📷 Camera reset to optimal drilling pattern view');
    }
  }

  // Auto-frame the drilling pattern
  autoFramePattern(): void {
    if (!this.drillData || this.drillData.length === 0) {
      this.resetCamera();
      return;
    }

    // Calculate bounding box of valid drill holes
    const validHoles = this.drillData.filter(h => h.easting > 0 && h.northing > 0);
    if (validHoles.length === 0) {
      this.resetCamera();
      return;
    }

    const eastings = validHoles.map(h => h.easting);
    const northings = validHoles.map(h => h.northing);
    const elevations = validHoles.map(h => h.elevation);

    const minEasting = Math.min(...eastings);
    const maxEasting = Math.max(...eastings);
    const minNorthing = Math.min(...northings);  
    const maxNorthing = Math.max(...northings);
    const minElevation = Math.min(...elevations);
    const maxElevation = Math.max(...elevations);

    // Convert to grid coordinates (relative to origin)
    const patternWidth = maxEasting - minEasting;
    const patternHeight = maxNorthing - minNorthing;
    const patternDepth = maxElevation - minElevation;

    // Calculate center of pattern in grid coordinates
    const centerX = patternWidth / 2;
    const centerZ = patternHeight / 2;
    const centerY = patternDepth / 2;

    // Calculate optimal viewing distance
    const maxDimension = Math.max(patternWidth, patternHeight, 20);
    const distance = maxDimension * 2.0;
    const height = Math.max(distance * 0.5, 40);

    // Position camera to frame the pattern
    this.camera.position.set(
      centerX + distance * 0.8,
      height,
      centerZ + distance * 0.8
    );

    this.controls.target.set(centerX, centerY, centerZ);
    this.controls.update();

    console.log(`📷 Auto-framed pattern: ${patternWidth.toFixed(1)}m × ${patternHeight.toFixed(1)}m`);
  }

  // Enable/disable auto-rotation
  toggleAutoRotate(): void {
    if (this.controls) {
      this.controls.autoRotate = !this.controls.autoRotate;
      console.log(`📷 Auto-rotate: ${this.controls.autoRotate ? 'ON' : 'OFF'}`);
    }
  }

  // Set preset camera views
  setTopView(): void {
    if (this.camera && this.controls) {
      this.camera.position.set(50, 150, 50);
      this.controls.target.set(50, 0, 50);
      this.controls.update();
      console.log('📷 Top view activated');
    }
  }

  setSideView(): void {
    if (this.camera && this.controls) {
      this.camera.position.set(150, 30, 50);
      this.controls.target.set(50, 0, 50);
      this.controls.update();
      console.log('📷 Side view activated');
    }
  }

  setIsometricView(): void {
    if (this.camera && this.controls) {
      this.camera.position.set(120, 80, 120);
      this.controls.target.set(50, 0, 50);
      this.controls.update();
      console.log('📷 Isometric view activated');
    }
  }

  // Three.js Methods
  private initThreeJS(): void {
    const container = this.el.nativeElement.querySelector('.threejs-container');
    if (!container) return;

    const width = container.clientWidth || 800;
    const height = container.clientHeight || 600;

    // Enhanced Scene Setup
    this.scene = new THREE.Scene();
    this.scene.background = new THREE.Color(0xf0f8ff);
    this.scene.fog = new THREE.Fog(0xf0f8ff, 100, 300); // Add fog for depth perception

    // Enhanced Camera with better FOV and range
    this.camera = new THREE.PerspectiveCamera(50, width / height, 0.1, 2000);
    
    // Enhanced Renderer with better quality settings
    this.renderer = new THREE.WebGLRenderer({
      antialias: true,
      alpha: true,
      powerPreference: "high-performance"
    });
    this.renderer.setSize(width, height);
    this.renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2));
    this.renderer.shadowMap.enabled = true;
    this.renderer.shadowMap.type = THREE.PCFSoftShadowMap;
    this.renderer.outputColorSpace = THREE.SRGBColorSpace;
    this.renderer.toneMapping = THREE.ACESFilmicToneMapping;
    this.renderer.toneMappingExposure = 1.2;
    container.appendChild(this.renderer.domElement);

    // Enhanced Controls
    this.controls = new OrbitControls(this.camera, this.renderer.domElement);
    this.setupEnhancedControls();
    
    // Set optimal initial camera position
    this.setOptimalCameraView();
  }

  private setupEnhancedControls(): void {
    // Smooth damping for fluid movement
    this.controls.enableDamping = true;
    this.controls.dampingFactor = 0.08;
    
    // Enhanced zoom controls
    this.controls.minDistance = 10;     // Minimum zoom distance
    this.controls.maxDistance = 400;    // Maximum zoom distance  
    this.controls.zoomSpeed = 0.8;      // Smooth zoom speed
    
    // Improved rotation controls
    this.controls.rotateSpeed = 0.6;    // Smooth rotation
    this.controls.maxPolarAngle = Math.PI * 0.9;  // Prevent going too far under
    this.controls.minPolarAngle = 0.05; // Prevent going directly overhead
    
    // Enhanced panning
    this.controls.panSpeed = 1.0;
    this.controls.screenSpacePanning = false; // Pan in world space
    
    // Auto-rotate option (can be enabled later)
    this.controls.autoRotate = false;
    this.controls.autoRotateSpeed = 1.0;
    
    // Smooth target following
    this.controls.target.set(50, 0, 50); // Center of 100x100 grid
    
    // Keyboard controls
    this.controls.listenToKeyEvents(window);
    
    // Mouse settings
    this.controls.mouseButtons = {
      LEFT: THREE.MOUSE.ROTATE,
      MIDDLE: THREE.MOUSE.DOLLY,
      RIGHT: THREE.MOUSE.PAN
    };
  }

  private setOptimalCameraView(): void {
    // Position camera for best overview of drilling pattern
    const gridCenter = 50; // Center of 100x100 grid
    const distance = 120;   // Optimal viewing distance
    const height = 60;      // Good elevation angle
    const angle = Math.PI / 6; // 30 degrees from north
    
    this.camera.position.set(
      gridCenter + distance * Math.cos(angle),
      height,
      gridCenter + distance * Math.sin(angle)
    );
    
    this.camera.lookAt(gridCenter, 0, gridCenter);
    this.controls.target.set(gridCenter, 0, gridCenter);
    this.controls.update();
  }

  private gridHelper?: THREE.GridHelper;
  private groundPlane?: THREE.Mesh;

  private createScene(): void {
    // Add initial grid (will be updated when drill data loads)
    this.gridHelper = new THREE.GridHelper(50, 50, 0x444444, 0x888888);
    this.scene.add(this.gridHelper);

    // Add custom coordinate axes with proper colors
    // Red = East (X), Green = North (Z), Blue = Up (Y)
    this.createCustomAxes();
    
    // Add lighting
    const ambientLight = new THREE.AmbientLight(0xffffff, 0.6);
    this.scene.add(ambientLight);

    const directionalLight = new THREE.DirectionalLight(0xffffff, 0.8);
    directionalLight.position.set(20, 20, 20);
    directionalLight.castShadow = true;
    directionalLight.shadow.mapSize.width = 2048;
    directionalLight.shadow.mapSize.height = 2048;
    this.scene.add(directionalLight);

    // Add initial ground plane (will be updated when drill data loads)
    const groundGeometry = new THREE.PlaneGeometry(50, 50);
    const groundMaterial = new THREE.MeshLambertMaterial({ 
      color: 0xcccccc, 
      transparent: true, 
      opacity: 0.3 
    });
    this.groundPlane = new THREE.Mesh(groundGeometry, groundMaterial);
    this.groundPlane.rotation.x = -Math.PI / 2;
    this.groundPlane.receiveShadow = true;
    this.scene.add(this.groundPlane);
  }

  private updateGridToMatchPattern(eastingRange: number, northingRange: number): void {
    // Fixed grid size of 100x100 meters
    const gridSize = 100; // 100 meters
    const gridDivisions = 50; // Grid lines every 2 meters (100m / 50 divisions = 2m per square)

    // Remove old grid and ground plane
    if (this.gridHelper) {
      this.scene.remove(this.gridHelper);
      this.gridHelper.dispose();
    }
    if (this.groundPlane) {
      this.scene.remove(this.groundPlane);
      this.groundPlane.geometry.dispose();
      if (this.groundPlane.material instanceof THREE.Material) {
        this.groundPlane.material.dispose();
      }
    }

    // Create new grid sized to match the drilling pattern
    this.gridHelper = new THREE.GridHelper(gridSize, gridDivisions, 0x444444, 0x888888);
    this.scene.add(this.gridHelper);

    // Create new ground plane sized to match the drilling pattern  
    const groundGeometry = new THREE.PlaneGeometry(gridSize, gridSize);
    const groundMaterial = new THREE.MeshLambertMaterial({ 
      color: 0xcccccc, 
      transparent: true, 
      opacity: 0.3 
    });
    this.groundPlane = new THREE.Mesh(groundGeometry, groundMaterial);
    this.groundPlane.rotation.x = -Math.PI / 2;
    this.groundPlane.receiveShadow = true;
    this.scene.add(this.groundPlane);

    console.log(`🔄 Fixed grid: ${gridSize}m × ${gridSize}m with ${gridDivisions} divisions (2m per square)`);
  }

  private createCustomAxes(): void {
    // Create custom axes with proper colors for drilling coordinates
    // Red = East (X-axis), Green = North (Z-axis), Blue = Up (Y-axis)
    
    const axisLength = 10;
    const axisWidth = 3;

    // East axis (X) - Red
    const eastGeometry = new THREE.CylinderGeometry(0.05, 0.05, axisLength, 8);
    const eastMaterial = new THREE.MeshBasicMaterial({ color: 0xff0000 });
    const eastAxis = new THREE.Mesh(eastGeometry, eastMaterial);
    eastAxis.rotation.z = -Math.PI / 2; // Rotate to align with X-axis
    eastAxis.position.x = axisLength / 2;
    this.scene.add(eastAxis);

    // North axis (Z) - Green  
    const northGeometry = new THREE.CylinderGeometry(0.05, 0.05, axisLength, 8);
    const northMaterial = new THREE.MeshBasicMaterial({ color: 0x00ff00 });
    const northAxis = new THREE.Mesh(northGeometry, northMaterial);
    northAxis.rotation.x = Math.PI / 2; // Rotate to align with Z-axis
    northAxis.position.z = axisLength / 2;
    this.scene.add(northAxis);

    // Up axis (Y) - Blue
    const upGeometry = new THREE.CylinderGeometry(0.05, 0.05, axisLength, 8);
    const upMaterial = new THREE.MeshBasicMaterial({ color: 0x0000ff });
    const upAxis = new THREE.Mesh(upGeometry, upMaterial);
    upAxis.position.y = axisLength / 2;
    this.scene.add(upAxis);
    
    // Add axis labels
    this.addAxisLabels();
  }

  private addAxisLabels(): void {
    // East label (Red)
    const eastLabel = this.createAxisLabel('EAST', 0xff0000);
    eastLabel.position.set(12, 0.5, 0);
    this.scene.add(eastLabel);

    // North label (Green)  
    const northLabel = this.createAxisLabel('NORTH', 0x00ff00);
    northLabel.position.set(0, 0.5, 12);
    this.scene.add(northLabel);

    // Up label (Blue)
    const upLabel = this.createAxisLabel('UP', 0x0000ff);
    upLabel.position.set(0, 12, 0);
    this.scene.add(upLabel);
  }

  private createAxisLabel(text: string, color: number): THREE.Sprite {
    const canvas = document.createElement('canvas');
    const context = canvas.getContext('2d');
    if (!context) {
      return new THREE.Sprite(new THREE.SpriteMaterial({ color: 0xffffff }));
    }

    canvas.width = 128;
    canvas.height = 32;
    
    // Background
    context.fillStyle = 'rgba(255, 255, 255, 0.9)';
    context.fillRect(0, 0, canvas.width, canvas.height);
    
    // Border
    context.strokeStyle = `#${color.toString(16).padStart(6, '0')}`;
    context.lineWidth = 2;
    context.strokeRect(0, 0, canvas.width, canvas.height);
    
    // Text
    context.fillStyle = `#${color.toString(16).padStart(6, '0')}`;
    context.font = 'bold 16px Arial';
    context.textAlign = 'center';
    context.textBaseline = 'middle';
    context.fillText(text, canvas.width/2, canvas.height/2);
    
    const texture = new THREE.CanvasTexture(canvas);
    const material = new THREE.SpriteMaterial({ map: texture });
    const label = new THREE.Sprite(material);
    label.scale.set(3, 0.75, 1);
    
    return label;
  }

  private orientDrillHole(drillObject: THREE.Object3D, azimuth: number, dip: number): void {
    // Reset rotations
    drillObject.rotation.set(0, 0, 0);
    
    // Convert angles to radians
    const azimuthRad = THREE.MathUtils.degToRad(azimuth);
    const dipRad = THREE.MathUtils.degToRad(dip);
    
    // Drill hole orientation in mining/surveying convention:
    // Azimuth: 0° = North, 90° = East, 180° = South, 270° = West
    // Dip: 0° = Horizontal, 90° = Straight down
    
    // 1. First tilt the hole according to dip (from vertical)
    // Cylinder is created vertically, so we tilt it from vertical
    drillObject.rotateZ(dipRad); // Tilt in the Z-X plane
    
    // 2. Then rotate horizontally according to azimuth
    // Azimuth 0° = North (+Z), rotate around Y-axis
    drillObject.rotateY(-azimuthRad); // Negative for correct direction
    
    console.log(`Oriented hole: Azimuth=${azimuth}° (${this.getAzimuthDirection(azimuth)}), Dip=${dip}°`);
  }

  private getAzimuthDirection(azimuth: number): string {
    const normalizedAzimuth = azimuth % 360;
    if (normalizedAzimuth >= 337.5 || normalizedAzimuth < 22.5) return 'North';
    if (normalizedAzimuth >= 22.5 && normalizedAzimuth < 67.5) return 'NorthEast';
    if (normalizedAzimuth >= 67.5 && normalizedAzimuth < 112.5) return 'East';
    if (normalizedAzimuth >= 112.5 && normalizedAzimuth < 157.5) return 'SouthEast';
    if (normalizedAzimuth >= 157.5 && normalizedAzimuth < 202.5) return 'South';
    if (normalizedAzimuth >= 202.5 && normalizedAzimuth < 247.5) return 'SouthWest';
    if (normalizedAzimuth >= 247.5 && normalizedAzimuth < 292.5) return 'West';
    if (normalizedAzimuth >= 292.5 && normalizedAzimuth < 337.5) return 'NorthWest';
    return 'Unknown';
  }

  private create3DDrillHoles(): void {
    if (!this.drillData || this.drillData.length === 0) return;

        // PRECISE METHOD: Convert UTM coordinates to local meter-based grid system
    
    // Step 1: Find the lowest coordinates (this becomes our origin 0,0,0)
    console.log(`🔍 Raw drill data check:`, this.drillData.map(h => ({
      id: h.id, 
      easting: h.easting, 
      northing: h.northing, 
      elevation: h.elevation
    })));
    
    // Ensure coordinates are numbers and filter out invalid 0.00 UTM coordinates
    const eastings = this.drillData.map(h => Number(h.easting)).filter(n => !isNaN(n) && n > 0);
    const northings = this.drillData.map(h => Number(h.northing)).filter(n => !isNaN(n) && n > 0);
    const elevations = this.drillData.map(h => Number(h.elevation)).filter(n => !isNaN(n) && n > 0);
    
    console.log(`🔍 Extracted coordinates:`, {
      eastings,
      northings,
      elevations
    });
    
    const originEasting = Math.min(...eastings);
    const originNorthing = Math.min(...northings);  
    const originElevation = Math.min(...elevations);
    
    console.log(`🔍 Calculated origins:`, {
      originEasting,
      originNorthing,
      originElevation
    });
    
    // Count valid vs invalid holes
    const validHoles = this.drillData.filter(h => h.easting > 0 && h.northing > 0 && h.elevation > 0);
    const invalidHoles = this.drillData.filter(h => h.easting <= 0 || h.northing <= 0 || h.elevation <= 0);
    
    console.log(`📊 Data Quality Check:
      ✅ Valid holes: ${validHoles.length}
      ❌ Invalid holes (0.00 coordinates): ${invalidHoles.length}
      📍 Invalid hole IDs: [${invalidHoles.map(h => h.id).join(', ')}]`);
    
    // Step 2: Calculate pattern dimensions in meters from origin
    const eastingRange = Math.max(...this.drillData.map(h => h.easting)) - originEasting;
    const northingRange = Math.max(...this.drillData.map(h => h.northing)) - originNorthing;
    const elevationRange = Math.max(...this.drillData.map(h => h.elevation)) - originElevation;
    
    // Step 3: Set 1:1 meter scale for grid positioning
    const meterScale = 1.0; // 1 UTM meter = 1 grid unit
    
    // Step 4: Update grid to 100x100 meter reference
    this.updateGridToMatchPattern(eastingRange, northingRange);

    console.log(`🎯 UTM TO GRID CONVERSION:
      🌍 UTM Origin: E=${originEasting.toFixed(2)}, N=${originNorthing.toFixed(2)}, Z=${originElevation.toFixed(2)}
      📏 Grid Origin: (0, 0, 0) meters
      📐 Pattern Size: ${eastingRange.toFixed(2)}m East × ${northingRange.toFixed(2)}m North × ${elevationRange.toFixed(2)}m Up
      📊 Total Holes: ${this.drillData.length}
      🔄 Conversion: UTM - Origin = Grid Meters
      🔍 All Eastings: [${this.drillData.map(h => h.easting.toFixed(2)).join(', ')}]
      🔍 All Northings: [${this.drillData.map(h => h.northing.toFixed(2)).join(', ')}]`);

    this.drillData.forEach((hole, index) => {
      // Skip holes with invalid coordinates (0.00 values)
      if (hole.easting <= 0 || hole.northing <= 0 || hole.elevation <= 0) {
        console.log(`⚠️ Skipping hole ${hole.id} - invalid coordinates: E=${hole.easting}, N=${hole.northing}, Z=${hole.elevation}`);
        return;
      }
      
      // Step 5: Position holes starting from grid corner (0,0) as zero reference
      // Convert UTM differences to meters, then position relative to grid corner
      const eastMetersFromCorner = (hole.easting - originEasting) * meterScale;
      const northMetersFromCorner = (hole.northing - originNorthing) * meterScale;
      const upMetersFromCorner = (hole.elevation - originElevation) * meterScale;
      
      // DEBUG: Show calculation step by step
      console.log(`🔧 Calculation for Hole ${hole.id}:
        📊 Hole UTM: E=${hole.easting}, N=${hole.northing}, Z=${hole.elevation}
        📊 Origin UTM: E=${originEasting}, N=${originNorthing}, Z=${originElevation}
        ➖ Difference: E=${(hole.easting - originEasting).toFixed(2)}, N=${(hole.northing - originNorthing).toFixed(2)}, Z=${(hole.elevation - originElevation).toFixed(2)}
        ✅ Grid Meters: E=${eastMetersFromCorner.toFixed(2)}, N=${northMetersFromCorner.toFixed(2)}, U=${upMetersFromCorner.toFixed(2)}`);
      
      // Position holes starting from grid corner (0,0,0) - bottom-left corner of grid
      // Grid extends from (0,0) to (100,100), so holes are positioned within this area
      const x = eastMetersFromCorner;   // X = East from corner (0 to 100m)
      const z = northMetersFromCorner;  // Z = North from corner (0 to 100m)  
      const y = upMetersFromCorner;     // Y = Up from corner base elevation
      
      // Step 6: Create drill hole with exact positioning - NO ADJUSTMENTS

      // Create drill hole group
    const drillGroup = new THREE.Group();
    
      // Create drill hole cylinder using actual depth in meters
      const depth = Math.max(hole.depth * meterScale, 0.5); // Minimum 0.5 meter depth for visibility
      const radius = 0.15; // Increased radius for better visibility
      
      const geometry = new THREE.CylinderGeometry(radius, radius, depth, 8);
      const material = new THREE.MeshPhongMaterial({
        color: this.getColorForHole(index),
        transparent: true,
        opacity: 0.8
      });
      
      const drillHole = new THREE.Mesh(geometry, material);
      
      // Position drill hole - cylinder center should be at depth/2 below collar
      drillHole.position.set(0, -depth/2, 0); // Relative to group origin
      
      // Apply proper drilling orientation
      this.orientDrillHole(drillHole, hole.azimuth, hole.dip);
      
      // Position the entire group at the collar location
      drillGroup.position.set(x, y, z);
      
      drillHole.castShadow = true;
      drillGroup.add(drillHole);

      // Collar markers removed per user request
    
      // Add label if enabled
      if (this.showLabels) {
        const label = this.showDetailedLabels 
          ? this.createDetailedLabel(hole)
          : this.createLabel(hole.id);
        label.position.set(0, 2, 0); // Positioned above the hole
        this.labelObjects.push(label);
        drillGroup.add(label); // Add to group instead of scene
      }

      this.drillObjects.push(drillGroup);
      this.scene.add(drillGroup);
      
      // Debug grid corner positioning information
      console.log(`✅ Hole ${hole.id}:
        🌍 UTM Coordinates: E=${hole.easting.toFixed(2)}, N=${hole.northing.toFixed(2)}, Z=${hole.elevation.toFixed(2)}
        📐 Distance from Grid Corner: East=${eastMetersFromCorner.toFixed(2)}m, North=${northMetersFromCorner.toFixed(2)}m, Up=${upMetersFromCorner.toFixed(2)}m
        🎯 Grid Position: X=${x.toFixed(2)}, Y=${y.toFixed(2)}, Z=${z.toFixed(2)}
        📏 Hole Depth: ${hole.depth}m (rendered: ${depth.toFixed(2)}m)
        🧭 Azimuth: ${hole.azimuth}° (${this.getAzimuthDirection(hole.azimuth)})
        📐 Dip: ${hole.dip}° downward
        🏗️ Stemming: ${hole.stemming}m`);
    });

    console.log(`Created ${this.drillObjects.length} 3D drill holes with proper positioning`);
    
    // Auto-frame the pattern for optimal viewing
    this.autoFramePattern();
  }

  private getColorForHole(index: number): number {
    // Get hole ID from drill data
    const holeId = this.drillData[index]?.id || '';
    const firstLetter = holeId.charAt(0).toUpperCase();
    
    // Assign colors based on first letter of hole name
    const letterColors: { [key: string]: number } = {
      'A': 0xff0000, // Red
      'B': 0x00ff00, // Green  
      'C': 0x0000ff, // Blue
      'D': 0xffff00, // Yellow
      'E': 0xff00ff, // Magenta
      'F': 0x00ffff, // Cyan
      'G': 0xffa500, // Orange
      'H': 0x800080, // Purple
      'I': 0xffc0cb, // Pink
      'J': 0x964b00, // Brown
      'K': 0x808080, // Gray
      'L': 0x00ff7f, // Spring Green
      'M': 0xff69b4, // Hot Pink
      'N': 0x4169e1, // Royal Blue
      'O': 0xdc143c, // Crimson
      'P': 0x32cd32, // Lime Green
      'Q': 0xffd700, // Gold
      'R': 0x8b0000, // Dark Red
      'S': 0x2e8b57, // Sea Green
      'T': 0x4682b4, // Steel Blue
      'U': 0xd2691e, // Chocolate
      'V': 0x9370db, // Medium Purple
      'W': 0x20b2aa, // Light Sea Green
      'X': 0xb22222, // Fire Brick
      'Y': 0xdaa520, // Goldenrod
      'Z': 0x483d8b  // Dark Slate Blue
    };
    
    // Return color for the letter, or default red if not found
    const color = letterColors[firstLetter] || 0xff0000;
    
    console.log(`🎨 Hole ${holeId}: Letter '${firstLetter}' → Color #${color.toString(16).padStart(6, '0')}`);
    
    return color;
  }

  private createLabel(text: string): THREE.Sprite {
    const canvas = document.createElement('canvas');
    const context = canvas.getContext('2d');
    if (!context) {
      return new THREE.Sprite(new THREE.SpriteMaterial({ color: 0xffffff }));
    }

    // Enhanced canvas size for better quality
    canvas.width = 200;
    canvas.height = 50;
    
    // Enable high-quality text rendering
    context.imageSmoothingEnabled = true;
    context.imageSmoothingQuality = 'high';
    
    // Create gradient background
    const gradient = context.createLinearGradient(0, 0, 0, canvas.height);
    gradient.addColorStop(0, 'rgba(70, 130, 180, 0.95)'); // Steel blue top
    gradient.addColorStop(1, 'rgba(25, 25, 112, 0.95)');  // Midnight blue bottom
    
    // Draw rounded rectangle background
    this.drawRoundedRect(context, 2, 2, canvas.width - 4, canvas.height - 4, 8, gradient);
    
    // Add border
    context.strokeStyle = 'rgba(255, 255, 255, 0.8)';
    context.lineWidth = 2;
    this.drawRoundedRectStroke(context, 2, 2, canvas.width - 4, canvas.height - 4, 8);
    
    // Add inner shadow effect
    context.shadowColor = 'rgba(0, 0, 0, 0.3)';
    context.shadowBlur = 4;
    context.shadowOffsetX = 1;
    context.shadowOffsetY = 1;
    
    // Enhanced text styling
    context.fillStyle = '#FFFFFF';
    context.font = 'bold 18px "Segoe UI", Arial, sans-serif';
    context.textAlign = 'center';
    context.textBaseline = 'middle';
    
    // Add text stroke for better readability
    context.strokeStyle = 'rgba(0, 0, 0, 0.5)';
    context.lineWidth = 3;
    context.strokeText(text, canvas.width/2, canvas.height/2);
    
    // Fill text
    context.fillText(text, canvas.width/2, canvas.height/2);
    
    // Reset shadow
    context.shadowColor = 'transparent';
    
    const texture = new THREE.CanvasTexture(canvas);
    texture.colorSpace = THREE.SRGBColorSpace;
    
    const material = new THREE.SpriteMaterial({ 
      map: texture,
      transparent: true,
      alphaTest: 0.1,
      depthTest: false,
      depthWrite: false
    });
    
    const label = new THREE.Sprite(material);
    label.scale.set(4, 1, 1); // Larger scale for better visibility
    
    return label;
  }

  // Helper method to draw rounded rectangles
  private drawRoundedRect(ctx: CanvasRenderingContext2D, x: number, y: number, width: number, height: number, radius: number, fillStyle: string | CanvasGradient): void {
    ctx.fillStyle = fillStyle;
    ctx.beginPath();
    ctx.moveTo(x + radius, y);
    ctx.lineTo(x + width - radius, y);
    ctx.quadraticCurveTo(x + width, y, x + width, y + radius);
    ctx.lineTo(x + width, y + height - radius);
    ctx.quadraticCurveTo(x + width, y + height, x + width - radius, y + height);
    ctx.lineTo(x + radius, y + height);
    ctx.quadraticCurveTo(x, y + height, x, y + height - radius);
    ctx.lineTo(x, y + radius);
    ctx.quadraticCurveTo(x, y, x + radius, y);
    ctx.closePath();
    ctx.fill();
  }

  // Helper method to stroke rounded rectangles
  private drawRoundedRectStroke(ctx: CanvasRenderingContext2D, x: number, y: number, width: number, height: number, radius: number): void {
    ctx.beginPath();
    ctx.moveTo(x + radius, y);
    ctx.lineTo(x + width - radius, y);
    ctx.quadraticCurveTo(x + width, y, x + width, y + radius);
    ctx.lineTo(x + width, y + height - radius);
    ctx.quadraticCurveTo(x + width, y + height, x + width - radius, y + height);
    ctx.lineTo(x + radius, y + height);
    ctx.quadraticCurveTo(x, y + height, x, y + height - radius);
    ctx.lineTo(x, y + radius);
    ctx.quadraticCurveTo(x, y, x + radius, y);
    ctx.closePath();
    ctx.stroke();
  }

  // Create enhanced labels with additional info
  private createDetailedLabel(hole: any): THREE.Sprite {
    const canvas = document.createElement('canvas');
    const context = canvas.getContext('2d');
    if (!context) {
      return new THREE.Sprite(new THREE.SpriteMaterial({ color: 0xffffff }));
    }

    canvas.width = 300;
    canvas.height = 80;
    
    context.imageSmoothingEnabled = true;
    context.imageSmoothingQuality = 'high';
    
    // Create gradient background
    const gradient = context.createLinearGradient(0, 0, 0, canvas.height);
    gradient.addColorStop(0, 'rgba(34, 139, 34, 0.95)'); // Forest green top
    gradient.addColorStop(1, 'rgba(0, 100, 0, 0.95)');   // Dark green bottom
    
    // Draw background
    this.drawRoundedRect(context, 2, 2, canvas.width - 4, canvas.height - 4, 10, gradient);
    
    // Add border
    context.strokeStyle = 'rgba(255, 255, 255, 0.9)';
    context.lineWidth = 2;
    this.drawRoundedRectStroke(context, 2, 2, canvas.width - 4, canvas.height - 4, 10);
    
    // Main title
    context.fillStyle = '#FFFFFF';
    context.font = 'bold 20px "Segoe UI", Arial, sans-serif';
    context.textAlign = 'center';
    context.textBaseline = 'top';
    
    // Add text stroke
    context.strokeStyle = 'rgba(0, 0, 0, 0.5)';
    context.lineWidth = 3;
    context.strokeText(hole.id, canvas.width/2, 8);
    context.fillText(hole.id, canvas.width/2, 8);
    
    // Subtitle with depth and azimuth
    context.font = 'normal 14px "Segoe UI", Arial, sans-serif';
    context.textAlign = 'center';
    context.textBaseline = 'bottom';
    
    const subtitle = `Depth: ${hole.depth}m | Az: ${hole.azimuth}°`;
    context.strokeText(subtitle, canvas.width/2, canvas.height - 8);
    context.fillText(subtitle, canvas.width/2, canvas.height - 8);
    
    const texture = new THREE.CanvasTexture(canvas);
    texture.colorSpace = THREE.SRGBColorSpace;
    
    const material = new THREE.SpriteMaterial({ 
      map: texture,
      transparent: true,
      alphaTest: 0.1,
      depthTest: false,
      depthWrite: false
    });
    
    const label = new THREE.Sprite(material);
    label.scale.set(6, 1.6, 1); // Larger scale for detailed info
    
    return label;
  }

  private clear3DDrillHoles(): void {
    this.drillObjects.forEach(obj => {
      this.scene.remove(obj);
      this.dispose3DObject(obj);
    });
    this.drillObjects = [];

    this.labelObjects.forEach(label => {
      this.scene.remove(label);
      if (label.material instanceof THREE.SpriteMaterial) {
        label.material.map?.dispose();
        label.material.dispose();
      }
    });
    this.labelObjects = [];
  }

  private dispose3DObject(obj: THREE.Object3D): void {
    if (obj instanceof THREE.Mesh) {
      obj.geometry.dispose();
      if (Array.isArray(obj.material)) {
        obj.material.forEach(material => material.dispose());
      } else {
        obj.material.dispose();
      }
    }
    obj.children.forEach(child => this.dispose3DObject(child));
  }

  private cleanup3D(): void {
    if (this.animationFrameId) {
      cancelAnimationFrame(this.animationFrameId);
    }
    
    this.clear3DDrillHoles();
    
    if (this.renderer) {
      this.renderer.dispose();
      const canvas = this.renderer.domElement;
      canvas.parentNode?.removeChild(canvas);
    }
  }

  private animate(): void {
    this.animationFrameId = requestAnimationFrame(() => this.animate());
    
    if (this.controls) {
      this.controls.update();
    }
    
    if (this.renderer && this.scene && this.camera) {
      this.renderer.render(this.scene, this.camera);
    }
  }

  @HostListener('window:resize', ['$event'])
  onWindowResize(): void {
    const container = this.el.nativeElement.querySelector('.threejs-container');
    if (!container || !this.camera || !this.renderer) return;

    const width = container.clientWidth;
    const height = container.clientHeight;

    this.camera.aspect = width / height;
    this.camera.updateProjectionMatrix();
    this.renderer.setSize(width, height);
  }

  // Helper methods for coordinate bounds
  getMinEasting(): number {
    if (this.drillData.length === 0) return 0;
    return Math.min(...this.drillData.map(hole => hole.easting));
  }

  getMaxEasting(): number {
    if (this.drillData.length === 0) return 0;
    return Math.max(...this.drillData.map(hole => hole.easting));
  }

  getMinNorthing(): number {
    if (this.drillData.length === 0) return 0;
    return Math.min(...this.drillData.map(hole => hole.northing));
  }

  getMaxNorthing(): number {
    if (this.drillData.length === 0) return 0;
    return Math.max(...this.drillData.map(hole => hole.northing));
  }

  getMinElevation(): number {
    if (this.drillData.length === 0) return 0;
    return Math.min(...this.drillData.map(hole => hole.elevation));
  }

  getMaxElevation(): number {
    if (this.drillData.length === 0) return 0;
    return Math.max(...this.drillData.map(hole => hole.elevation));
  }
}
