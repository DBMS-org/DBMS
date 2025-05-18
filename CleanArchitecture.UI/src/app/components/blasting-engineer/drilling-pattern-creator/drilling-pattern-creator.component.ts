import { Component, ElementRef, ViewChild, AfterViewInit, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface DrillPoint {
  x: number;
  y: number;
  id: string;
  depth: number;
  spacing: number;
  burden: number;
}

@Component({
  selector: 'app-drilling-pattern-creator',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './drilling-pattern-creator.component.html',
  styleUrls: ['./drilling-pattern-creator.component.scss']
})
export class DrillingPatternCreatorComponent implements AfterViewInit {
  @ViewChild('canvas') canvasRef!: ElementRef<HTMLCanvasElement>;
  private ctx!: CanvasRenderingContext2D;
  private isDrawing = false;
  private isDragging = false;
  private dragStartX = 0;
  private dragStartY = 0;
  public selectedPoint: DrillPoint | null = null;
  public drillPoints: DrillPoint[] = [];
  private gridSize = 20;
  private scale = 1;
  private offsetX = 0;
  private offsetY = 0;
  public isHolePlacementMode = false;
  public showInstructions = true;

  // Pattern settings
  spacing = 3; // meters
  burden = 2.5; // meters
  depth = 10; // meters
  currentId = 1;

  ngAfterViewInit() {
    const canvas = this.canvasRef.nativeElement;
    this.ctx = canvas.getContext('2d')!;
    this.resizeCanvas();
    this.drawGrid();
  }

  @HostListener('window:resize')
  onResize() {
    this.resizeCanvas();
    this.drawGrid();
  }

  private resizeCanvas() {
    const canvas = this.canvasRef.nativeElement;
    canvas.width = canvas.offsetWidth;
    canvas.height = canvas.offsetHeight;
  }

  private drawRulers() {
    const canvas = this.canvasRef.nativeElement;
    const ctx = this.ctx;
    
    // Ruler dimensions
    const rulerWidth = 40;
    const rulerHeight = 40;
    
    // Draw ruler backgrounds
    ctx.fillStyle = '#f0f0f0';
    ctx.fillRect(0, 0, rulerWidth, canvas.height); // Vertical ruler
    ctx.fillRect(0, 0, canvas.width, rulerHeight); // Horizontal ruler
    
    // Draw ruler borders
    ctx.strokeStyle = '#ccc';
    ctx.lineWidth = 1;
    ctx.beginPath();
    ctx.moveTo(rulerWidth, 0);
    ctx.lineTo(rulerWidth, canvas.height);
    ctx.moveTo(0, rulerHeight);
    ctx.lineTo(canvas.width, rulerHeight);
    ctx.stroke();

    // Set text style for measurements
    ctx.fillStyle = '#333';
    ctx.font = '12px Arial';
    ctx.textAlign = 'center';
    ctx.textBaseline = 'middle';

    // Draw vertical ruler measurements based on burden
    const burdenInPixels = this.burden * this.gridSize * this.scale;
    let burdenValue = 0;
    for (let y = rulerHeight; y < canvas.height; y += burdenInPixels) {
      // Draw tick
      ctx.beginPath();
      ctx.moveTo(rulerWidth - 5, y);
      ctx.lineTo(rulerWidth, y);
      ctx.stroke();
      
      // Draw measurement
      ctx.fillText(`${burdenValue}m`, rulerWidth / 2, y);
      burdenValue += this.burden;
    }

    // Draw horizontal ruler measurements based on spacing
    const spacingInPixels = this.spacing * this.gridSize * this.scale;
    let spacingValue = 0;
    for (let x = rulerWidth; x < canvas.width; x += spacingInPixels) {
      // Draw tick
      ctx.beginPath();
      ctx.moveTo(x, rulerHeight - 5);
      ctx.lineTo(x, rulerHeight);
      ctx.stroke();
      
      // Draw measurement
      ctx.fillText(`${spacingValue}m`, x, rulerHeight / 2);
      spacingValue += this.spacing;
    }
  }

  private drawGrid() {
    const canvas = this.canvasRef.nativeElement;
    const ctx = this.ctx;
    
    // Ruler dimensions
    const rulerWidth = 40;
    const rulerHeight = 40;
    
    // Clear the entire canvas first
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    
    // Draw rulers first
    this.drawRulers();
    
    // Draw grid
    ctx.strokeStyle = '#ddd';
    ctx.lineWidth = 0.5;

    // Calculate the starting positions considering offset and scale
    const startX = rulerWidth + (this.offsetX % (this.spacing * this.gridSize * this.scale));
    const startY = rulerHeight + (this.offsetY % (this.burden * this.gridSize * this.scale));

    // Draw vertical grid lines with dynamic size based on spacing
    const spacingInPixels = this.spacing * this.gridSize * this.scale;
    for (let x = startX; x < canvas.width; x += spacingInPixels) {
      ctx.beginPath();
      ctx.moveTo(x, rulerHeight);
      ctx.lineTo(x, canvas.height);
      ctx.stroke();
    }

    // Draw horizontal grid lines with dynamic size based on burden
    const burdenInPixels = this.burden * this.gridSize * this.scale;
    for (let y = startY; y < canvas.height; y += burdenInPixels) {
      ctx.beginPath();
      ctx.moveTo(rulerWidth, y);
      ctx.lineTo(canvas.width, y);
      ctx.stroke();
    }

    this.drawDrillPoints();
  }

  private drawDrillPoints() {
    const rulerWidth = 40;
    const rulerHeight = 40;

    this.drillPoints.forEach(point => {
      this.ctx.beginPath();
      this.ctx.arc(
        point.x * this.scale + this.offsetX + rulerWidth,
        point.y * this.scale + this.offsetY + rulerHeight,
        5,
        0,
        Math.PI * 2
      );
      
      this.ctx.fillStyle = point === this.selectedPoint ? '#ff0000' : '#2196f3';
      this.ctx.fill();
      
      this.ctx.strokeStyle = '#ffffff';
      this.ctx.lineWidth = 2;
      this.ctx.stroke();

      this.ctx.fillStyle = '#000000';
      this.ctx.font = 'bold 12px Arial';
      this.ctx.fillText(
        point.id,
        point.x * this.scale + this.offsetX + rulerWidth + 8,
        point.y * this.scale + this.offsetY + rulerHeight + 4
      );
    });
  }

  private getCanvasCoordinates(event: MouseEvent): { x: number; y: number } {
    const canvas = this.canvasRef.nativeElement;
    const rect = canvas.getBoundingClientRect();
    const rulerWidth = 40;
    const rulerHeight = 40;
    
    return {
      x: (event.clientX - rect.left - this.offsetX - rulerWidth) / this.scale,
      y: (event.clientY - rect.top - this.offsetY - rulerHeight) / this.scale
    };
  }

  private findPointAtPosition(x: number, y: number): DrillPoint | null {
    return this.drillPoints.find(point => {
      const dx = point.x - x;
      const dy = point.y - y;
      return Math.sqrt(dx * dx + dy * dy) < 5 / this.scale;
    }) || null;
  }

  private updateCursor(event: MouseEvent) {
    const canvas = this.canvasRef.nativeElement;
    const { x, y } = this.getCanvasCoordinates(event);
    const pointUnderCursor = this.findPointAtPosition(x, y);

    if (this.isHolePlacementMode) {
      // In hole placement mode
      if (pointUnderCursor) {
        canvas.style.cursor = 'pointer'; // Show pointer when hovering over existing hole
      } else {
        canvas.style.cursor = 'crosshair'; // Show crosshair for placing new holes
      }
    } else {
      // Not in hole placement mode
      if (pointUnderCursor) {
        canvas.style.cursor = 'move'; // Show move cursor when hovering over existing hole
      } else if (event.altKey) {
        canvas.style.cursor = 'grab'; // Show grab cursor when holding alt
      } else {
        canvas.style.cursor = 'default'; // Default cursor otherwise
      }
    }
  }

  @HostListener('mousemove', ['$event'])
  onMouseMove(event: MouseEvent) {
    this.updateCursor(event); // Update cursor on mouse move

    if (this.isDrawing) {
      // Panning
      const dx = event.clientX - this.dragStartX;
      const dy = event.clientY - this.dragStartY;
      this.offsetX += dx;
      this.offsetY += dy;
      this.dragStartX = event.clientX;
      this.dragStartY = event.clientY;
      this.drawGrid();
    } else if (this.isDragging && this.selectedPoint) {
      // Dragging a point
      const { x, y } = this.getCanvasCoordinates(event);
      this.selectedPoint.x = x;
      this.selectedPoint.y = y;
      this.drawGrid();
    }
  }

  private isWithinRulerArea(x: number, y: number): boolean {
    const rulerWidth = 0;
    const rulerHeight = 0;
    return x < rulerWidth || y < rulerHeight;
  }

  @HostListener('mousedown', ['$event'])
  onMouseDown(event: MouseEvent) {
    const { x, y } = this.getCanvasCoordinates(event);
    
    if (event.button === 1 || (event.button === 0 && event.altKey)) {
      // Pan mode
      this.isDrawing = true;
      this.dragStartX = event.clientX;
      this.dragStartY = event.clientY;
      const canvas = this.canvasRef.nativeElement;
      canvas.style.cursor = 'grabbing';
    } else if (event.button === 0) {
      // Left click
      const clickedPoint = this.findPointAtPosition(x, y);
      
      if (this.isHolePlacementMode) {
        if (clickedPoint) {
          this.selectedPoint = clickedPoint;
        } else if (!this.isWithinRulerArea(x, y)) {
          // Add new point only if not in ruler area
          const newPoint: DrillPoint = {
            x,
            y,
            id: `DH${this.currentId++}`,
            depth: this.depth,
            spacing: this.spacing,
            burden: this.burden
          };
          this.drillPoints.push(newPoint);
          this.selectedPoint = newPoint;
        }
        this.drawGrid();
      } else {
        // Not in placement mode, just select or start dragging
        if (clickedPoint) {
          this.selectedPoint = clickedPoint;
          this.isDragging = true;
          this.dragStartX = x;
          this.dragStartY = y;
          const canvas = this.canvasRef.nativeElement;
          canvas.style.cursor = 'move';
        } else {
          this.selectedPoint = null;
        }
        this.drawGrid();
      }
    }
  }

  @HostListener('mouseup')
  onMouseUp() {
    this.isDrawing = false;
    this.isDragging = false;
    const canvas = this.canvasRef.nativeElement;
    this.updateCursor({ clientX: 0, clientY: 0 } as MouseEvent); // Reset cursor
  }

  @HostListener('keydown', ['$event'])
  onKeyDown(event: KeyboardEvent) {
    if (event.key === 'Alt') {
      const canvas = this.canvasRef.nativeElement;
      canvas.style.cursor = 'grab';
    }
  }

  @HostListener('keyup', ['$event'])
  onKeyUp(event: KeyboardEvent) {
    if (event.key === 'Alt') {
      const canvas = this.canvasRef.nativeElement;
      this.updateCursor({ clientX: 0, clientY: 0 } as MouseEvent);
    }
  }

  @HostListener('wheel', ['$event'])
  onMouseWheel(event: WheelEvent) {
    event.preventDefault();
    const delta = event.deltaY > 0 ? 0.9 : 1.1;
    this.scale *= delta;
    this.drawGrid();
  }

  toggleHolePlacementMode() {
    this.isHolePlacementMode = !this.isHolePlacementMode;
    const canvas = this.canvasRef.nativeElement;
    canvas.style.cursor = this.isHolePlacementMode ? 'crosshair' : 'default';
    this.selectedPoint = null;
    this.drawGrid();
  }

  onDeletePoint() {
    if (this.selectedPoint) {
      const index = this.drillPoints.indexOf(this.selectedPoint);
      if (index > -1) {
        this.drillPoints.splice(index, 1);
        this.selectedPoint = null;
        this.drawGrid();
      }
    }
  }

  onClearAll() {
    this.drillPoints = [];
    this.selectedPoint = null;
    this.drawGrid();
  }

  onExportPattern() {
    const pattern = {
      drillPoints: this.drillPoints,
      settings: {
        spacing: this.spacing,
        burden: this.burden,
        depth: this.depth
      }
    };
    
    const blob = new Blob([JSON.stringify(pattern, null, 2)], { type: 'application/json' });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = 'drilling-pattern.json';
    a.click();
    window.URL.revokeObjectURL(url);
  }

  toggleInstructions() {
    this.showInstructions = !this.showInstructions;
    // Add a small delay to allow the DOM to update
    setTimeout(() => {
      this.resizeCanvas();
      this.drawGrid();
    }, 0);
  }
}
