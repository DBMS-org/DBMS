import { Component, Input, ElementRef, OnInit, AfterViewInit, OnDestroy, HostListener } from '@angular/core';
import * as THREE from 'three';
import { OrbitControls } from 'three/addons/controls/OrbitControls.js';
import { CommonModule } from '@angular/common';

export interface DrillHole {
  id: string;
  east: number;
  north: number;
  elevation: number;
  azimuth: number;
  dip: number;
  depth: number;
  rockType?: string;
  mineralContent?: number;
}

@Component({
  selector: 'app-drill-visualization',
  templateUrl: './drill-visualization.component.html',
  styleUrls: ['./drill-visualization.component.scss'],
  standalone: true,
  imports: [CommonModule]
})
export class DrillVisualizationComponent implements OnInit, AfterViewInit, OnDestroy {
  @Input() drillData: DrillHole[] = [];
  
  showLabels = true;
  private colorScheme = 'random';
  private scene!: THREE.Scene;
  private camera!: THREE.PerspectiveCamera;
  private renderer!: THREE.WebGLRenderer;
  private controls!: OrbitControls;
  private drillObjects: THREE.Object3D[] = [];
  private labelObjects: THREE.Sprite[] = [];
  private centerPoint: THREE.Vector3 = new THREE.Vector3();
  private animationFrameId?: number;
  private resizeObserver?: ResizeObserver;

  constructor(private el: ElementRef) {}

  ngOnInit(): void {
    this.initThreeJS();
  }

  ngAfterViewInit(): void {
    this.createScene();
    this.animate();
    
    // Use ResizeObserver instead of window event for more accurate container sizing
    this.resizeObserver = new ResizeObserver(() => this.onResize());
    const container = this.el.nativeElement.querySelector('.renderer-container');
    if (container) {
      this.resizeObserver.observe(container);
    }
  }

  ngOnDestroy(): void {
    // Clean up resources
    if (this.animationFrameId) {
      cancelAnimationFrame(this.animationFrameId);
    }
    
    if (this.resizeObserver) {
      this.resizeObserver.disconnect();
    }
    
    // Dispose of Three.js objects
    this.drillObjects.forEach(object => {
      if (object instanceof THREE.Mesh) {
        object.geometry.dispose();
        if (Array.isArray(object.material)) {
          object.material.forEach(material => material.dispose());
        } else {
          object.material.dispose();
        }
      }
    });
    
    this.labelObjects.forEach(label => {
      if (label.material instanceof THREE.SpriteMaterial) {
        label.material.map?.dispose();
        label.material.dispose();
      }
    });
    
    // Remove the canvas element
    const rendererDom = this.renderer.domElement;
    rendererDom.parentNode?.removeChild(rendererDom);
  }

  resetCamera(): void {
    const container = this.el.nativeElement.querySelector('.renderer-container');
    if (!container) return;
    
    // Reset camera position
    this.camera.position.set(10, 10, 10);
    this.camera.lookAt(this.centerPoint);
    this.controls.target.copy(this.centerPoint);
    this.controls.update();
  }

  toggleLabels(): void {
    this.showLabels = !this.showLabels;
    this.labelObjects.forEach(label => {
      label.visible = this.showLabels;
    });
  }

  changeColorScheme(event: Event): void {
    this.colorScheme = (event.target as HTMLSelectElement).value;
    this.updateDrillColors();
  }

  private updateDrillColors(): void {
    if (!this.drillData || this.drillData.length === 0) return;
    
    // Find min/max values for depth-based coloring
    let minDepth = Infinity;
    let maxDepth = -Infinity;
    this.drillData.forEach(hole => {
      minDepth = Math.min(minDepth, hole.depth);
      maxDepth = Math.max(maxDepth, hole.depth);
    });
    
    // Get unique rock types if available
    const rockTypes = new Set<string>();
    this.drillData.forEach(hole => {
      if (hole.rockType) rockTypes.add(hole.rockType);
    });
    const rockTypeArray = Array.from(rockTypes);
    
    // Update colors for each drill hole
    this.drillObjects.forEach((object, index) => {
      if (!(object instanceof THREE.Mesh)) return;
      
      const hole = this.drillData[index];
      let color: THREE.Color;
      
      switch (this.colorScheme) {
        case 'depth':
          // Normalize depth between 0-1
          const normalizedDepth = (hole.depth - minDepth) / (maxDepth - minDepth || 1);
          // Blue to red gradient based on depth
          color = new THREE.Color().setHSL(0.6 * (1 - normalizedDepth), 0.8, 0.5);
          break;
        
        case 'rockType':
          if (hole.rockType && rockTypeArray.length > 0) {
            // Generate consistent color based on rock type
            const rockIndex = rockTypeArray.indexOf(hole.rockType);
            color = new THREE.Color().setHSL(rockIndex / rockTypeArray.length, 0.7, 0.5);
          } else {
            // Fallback to gray if no rock type data
            color = new THREE.Color(0x888888);
          }
          break;
        
        case 'random':
        default:
          // Use consistent random color based on hole ID
          const hash = this.hashString(hole.id);
          color = new THREE.Color().setHSL(hash % 1, 0.7, 0.5);
          break;
      }
      
      if (object.material instanceof THREE.MeshStandardMaterial) {
        object.material.color = color;
      }
    });
  }

  private hashString(str: string): number {
    let hash = 0;
    for (let i = 0; i < str.length; i++) {
      hash = ((hash << 5) - hash) + str.charCodeAt(i);
      hash |= 0; // Convert to 32bit integer
    }
    return Math.abs(hash) / 2147483647; // Normalize between 0-1
  }

  private initThreeJS(): void {
    const container = this.el.nativeElement.querySelector('.renderer-container');
    const width = container?.clientWidth || window.innerWidth;
    const height = container?.clientHeight || window.innerHeight;
    
    // Initialize scene with better lighting
    this.scene = new THREE.Scene();
    this.scene.background = new THREE.Color(0xf0f0f0);
    
    // Create fog for depth perception
    this.scene.fog = new THREE.FogExp2(0xf0f0f0, 0.01);

    // Initialize camera with proper aspect ratio
    this.camera = new THREE.PerspectiveCamera(
      60,
      width / height,
      0.1,
      1000
    );
    this.camera.position.set(10, 10, 10);

    // Initialize renderer with better defaults
    this.renderer = new THREE.WebGLRenderer({
      antialias: true,
      alpha: true,
      preserveDrawingBuffer: true // Allows for screenshots
    });
    this.renderer.setPixelRatio(window.devicePixelRatio);
    this.renderer.setSize(width, height);
    this.renderer.shadowMap.enabled = true;
    this.renderer.shadowMap.type = THREE.PCFSoftShadowMap;
    container?.appendChild(this.renderer.domElement);

    // Add improved controls
    this.controls = new OrbitControls(this.camera, this.renderer.domElement);
    this.controls.enableDamping = true;
    this.controls.dampingFactor = 0.05;
    this.controls.rotateSpeed = 0.8;
    this.controls.zoomSpeed = 1.2;
    this.controls.panSpeed = 0.8;
    this.controls.screenSpacePanning = true;
  }

  @HostListener('window:keydown', ['$event'])
  handleKeyDown(event: KeyboardEvent): void {
    // Add keyboard shortcuts
    switch (event.key.toLowerCase()) {
      case 'r':
        this.resetCamera();
        break;
      case 'l':
        this.toggleLabels();
        break;
    }
  }

  private onResize(): void {
    const container = this.el.nativeElement.querySelector('.renderer-container');
    if (!container) return;
    
    const width = container.clientWidth;
    const height = container.clientHeight;
    
    this.camera.aspect = width / height;
    this.camera.updateProjectionMatrix();
    this.renderer.setSize(width, height);
  }

  private createScene(): void {
    // Add grid helper
    const gridHelper = new THREE.GridHelper(100, 100, 0x888888, 0xcccccc);
    this.scene.add(gridHelper);

    // Add axes helper with labels
    const axesHelper = new THREE.AxesHelper(10);
    this.scene.add(axesHelper);
    
    // Add axis labels
    this.addAxisLabel('X', new THREE.Vector3(11, 0, 0), 'red');
    this.addAxisLabel('Y', new THREE.Vector3(0, 11, 0), 'green');
    this.addAxisLabel('Z', new THREE.Vector3(0, 0, 11), 'blue');

    // Add enhanced lighting
    const ambientLight = new THREE.AmbientLight(0xffffff, 0.5);
    this.scene.add(ambientLight);

    const mainLight = new THREE.DirectionalLight(0xffffff, 0.8);
    mainLight.position.set(10, 20, 15);
    mainLight.castShadow = true;
    // Optimize shadow map
    mainLight.shadow.mapSize.width = 2048;
    mainLight.shadow.mapSize.height = 2048;
    mainLight.shadow.camera.near = 0.5;
    mainLight.shadow.camera.far = 50;
    this.scene.add(mainLight);
    
    // Add fill light from opposite direction
    const fillLight = new THREE.DirectionalLight(0xffffcc, 0.3);
    fillLight.position.set(-10, 5, -10);
    this.scene.add(fillLight);

    // Create drill holes visualization
    this.createDrillHoles();
  }

  private addAxisLabel(text: string, position: THREE.Vector3, color: string): void {
    const canvas = document.createElement('canvas');
    const context = canvas.getContext('2d');
    if (!context) return;
    
    canvas.width = 64;
    canvas.height = 64;
    context.fillStyle = 'rgba(255, 255, 255, 0.8)';
    context.fillRect(0, 0, canvas.width, canvas.height);
    context.fillStyle = color;
    context.font = 'bold 48px Arial';
    context.textAlign = 'center';
    context.textBaseline = 'middle';
    context.fillText(text, canvas.width/2, canvas.height/2);
    
    const texture = new THREE.CanvasTexture(canvas);
    const material = new THREE.SpriteMaterial({ map: texture });
    const label = new THREE.Sprite(material);
    label.scale.set(1, 1, 1);
    label.position.copy(position);
    this.scene.add(label);
  }

  private createDrillHoles(): void {
    if (!this.drillData || this.drillData.length === 0) return;

    // Calculate bounds for camera positioning
    let minX = Infinity, maxX = -Infinity;
    let minY = Infinity, maxY = -Infinity;
    let minZ = Infinity, maxZ = -Infinity;

    this.drillData.forEach((hole: DrillHole) => {
      minX = Math.min(minX, hole.east);
      maxX = Math.max(maxX, hole.east);
      minY = Math.min(minY, hole.north);
      maxY = Math.max(maxY, hole.north);
      minZ = Math.min(minZ, hole.elevation);
      maxZ = Math.max(maxZ, hole.elevation);
    });

    // Center the scene
    const centerX = (minX + maxX) / 2;
    const centerY = (minY + maxY) / 2;
    const centerZ = (minZ + maxZ) / 2;
    
    this.centerPoint.set(0, 0, 0); // Center of the scene

    // Create a group for all drill holes
    const drillGroup = new THREE.Group();
    this.scene.add(drillGroup);

    this.drillData.forEach((hole: DrillHole, index: number) => {
      // Create instanced drill hole with improved geometry
      const drillHole = this.createDrillHoleObject(hole, centerX, centerY, centerZ, index);
      drillGroup.add(drillHole);
      this.drillObjects.push(drillHole);
    });

    // Position camera to view all holes
    const maxDimension = Math.max(
      maxX - minX,
      maxY - minY,
      maxZ - minZ
    );
    const distance = maxDimension * 1.5;
    this.camera.position.set(distance, distance, distance);
    this.camera.lookAt(this.centerPoint);
    this.controls.target.copy(this.centerPoint);
    this.controls.update();
  }

  private createDrillHoleObject(
    hole: DrillHole,
    centerX: number,
    centerY: number,
    centerZ: number,
    index: number
  ): THREE.Object3D {
    // Create a group for this drill hole and its components
    const drillGroup = new THREE.Group();
    
    // Create cylinder for the drill hole
    const geometry = new THREE.CylinderGeometry(0.2, 0.2, hole.depth, 16);
    // Shift origin to bottom of cylinder
    geometry.translate(0, hole.depth / 2, 0);
    
    // Create material with color based on hole ID for consistency
    const hash = this.hashString(hole.id);
    const material = new THREE.MeshStandardMaterial({ 
      color: new THREE.Color().setHSL(hash, 0.7, 0.5),
      metalness: 0.3,
      roughness: 0.7
    });
    
    const cylinder = new THREE.Mesh(geometry, material);
    cylinder.castShadow = true;
    cylinder.receiveShadow = true;
    
    // Create a collar (top of drill hole) with larger radius
    const collarGeometry = new THREE.CylinderGeometry(0.4, 0.4, 0.5, 16);
    collarGeometry.translate(0, 0.25, 0);
    const collarMaterial = new THREE.MeshStandardMaterial({ 
      color: 0xdddddd,
      metalness: 0.5,
      roughness: 0.2
    });
    const collar = new THREE.Mesh(collarGeometry, collarMaterial);
    collar.castShadow = true;
    
    // Add cylinder and collar to group
    drillGroup.add(cylinder);
    drillGroup.add(collar);
    
    // Position the group
    drillGroup.position.set(
      hole.east - centerX,
      hole.elevation - centerZ,
      hole.north - centerY
    );

    // Apply rotation based on azimuth and dip
    const azimuthRad = THREE.MathUtils.degToRad(hole.azimuth);
    const dipRad = THREE.MathUtils.degToRad(hole.dip);
    
    // First rotate around Y axis for azimuth
    drillGroup.rotateY(azimuthRad);
    // Then rotate around X axis for dip
    drillGroup.rotateX(dipRad);

    // Add hole ID label with enhanced appearance
    const label = this.createHoleLabel(hole);
    label.position.set(0, 1, 0);
    drillGroup.add(label);
    this.labelObjects.push(label);

    return drillGroup;
  }

  private createHoleLabel(hole: DrillHole): THREE.Sprite {
    const canvas = document.createElement('canvas');
    const context = canvas.getContext('2d');
    if (!context) {
      // Fallback if context not available
      const material = new THREE.SpriteMaterial({ color: 0xffffff });
      return new THREE.Sprite(material);
    }
    
    // Create larger canvas for better resolution
    canvas.width = 512;
    canvas.height = 128;
    
    // Draw background with rounded corners
    context.fillStyle = 'rgba(0, 0, 0, 0.7)';
    this.roundRect(context, 0, 0, canvas.width, canvas.height, 20);
    
    // Draw border
    context.strokeStyle = 'white';
    context.lineWidth = 4;
    this.roundRect(context, 2, 2, canvas.width-4, canvas.height-4, 18, false, true);
    
    // Add text with hole ID and basic info
    context.fillStyle = 'white';
    context.font = 'bold 48px Arial';
    context.textAlign = 'center';
    context.textBaseline = 'middle';
    context.fillText(hole.id, canvas.width/2, canvas.height/3);
    
    // Add depth information
    context.font = '24px Arial';
    context.fillText(`Depth: ${hole.depth.toFixed(1)}m`, canvas.width/2, canvas.height*2/3);
    
    const texture = new THREE.CanvasTexture(canvas);
    const material = new THREE.SpriteMaterial({ 
      map: texture,
      transparent: true,
      depthTest: false // Always show labels on top
    });
    const label = new THREE.Sprite(material);
    label.scale.set(4, 1, 1);
    
    return label;
  }

  private roundRect(
    ctx: CanvasRenderingContext2D,
    x: number,
    y: number,
    width: number,
    height: number,
    radius: number,
    fill: boolean = true,
    stroke: boolean = false
  ): void {
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
    if (fill) {
      ctx.fill();
    }
    if (stroke) {
      ctx.stroke();
    }
  }

  private animate(): void {
    this.animationFrameId = requestAnimationFrame(() => this.animate());
    this.controls.update();
    this.renderer.render(this.scene, this.camera);
  }
}
