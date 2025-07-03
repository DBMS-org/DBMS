import { Component, Input, ViewChild, ElementRef, OnChanges, SimpleChanges } from '@angular/core';
import { TimelineMarker } from '../../../shared/models/simulation.model';

@Component({
  selector: 'app-timeline',
  standalone: true,
  template: `
    <canvas #timelineCanvas width="800" height="100"></canvas>
  `,
  styles: [`
    canvas {
      width: 100%;
      height: 100px;
      background: #f8f9fa;
    }
  `]
})
export class TimelineComponent implements OnChanges {
  @ViewChild('timelineCanvas') timelineCanvasRef!: ElementRef<HTMLCanvasElement>;
  @Input() markers: TimelineMarker[] = [];
  @Input() currentTime: number = 0;
  @Input() totalDuration: number = 0;

  private ctx!: CanvasRenderingContext2D;

  ngOnChanges(changes: SimpleChanges): void {
    if (this.timelineCanvasRef) {
      this.ctx = this.timelineCanvasRef.nativeElement.getContext('2d')!;
      this.render();
    }
  }

  private render(): void {
    if (!this.ctx || this.totalDuration === 0) return;

    // Clear timeline
    this.ctx.clearRect(0, 0, 800, 100);
    
    // Draw time grid
    this.drawTimeGrid();

    // Group markers by type
    const waveMarkers = this.markers.filter(m => m.type === 'hole_blast' && m.label.includes('Wave'));
    const milestoneMarkers = this.markers.filter(m => m.type === 'milestone');
    const sequenceMarkers = this.markers.filter(m => m.type === 'sequence_start' || m.type === 'sequence_end');
    const detonationMarkers = this.markers.filter(m => m.type === 'hole_blast' && m.label.includes('Detonation'));

    // Render markers
    milestoneMarkers.forEach(marker => this.renderMarker(marker, 'small'));
    waveMarkers.forEach(marker => this.renderMarker(marker, 'medium'));
    detonationMarkers.forEach(marker => this.renderMarker(marker, 'medium'));
    sequenceMarkers.forEach(marker => this.renderMarker(marker, 'large'));

    // Render current time indicator
    this.renderCurrentTimeIndicator();
  }

  private drawTimeGrid(): void {
    this.ctx.strokeStyle = '#e9ecef';
    this.ctx.lineWidth = 1;
    
    const interval = 500; // 500ms intervals
    for (let time = 0; time <= this.totalDuration; time += interval) {
      const x = (time / this.totalDuration) * 780 + 10;
      this.ctx.beginPath();
      this.ctx.moveTo(x, 85);
      this.ctx.lineTo(x, 90);
      this.ctx.stroke();
      
      if (time % 1000 === 0) {
        this.ctx.fillStyle = '#6c757d';
        this.ctx.font = '9px Arial';
        this.ctx.textAlign = 'center';
        this.ctx.fillText(`${time/1000}s`, x, 98);
      }
    }
  }

  private renderMarker(marker: TimelineMarker, size: 'small' | 'medium' | 'large' = 'medium'): void {
    const x = (marker.time / this.totalDuration) * 780 + 10;
    const config = this.getMarkerConfig(size, marker.type);

    // Marker line
    this.ctx.beginPath();
    this.ctx.moveTo(x, config.lineStart);
    this.ctx.lineTo(x, config.lineEnd);
    this.ctx.strokeStyle = marker.color;
    this.ctx.lineWidth = config.lineWidth;
    this.ctx.stroke();

    // Marker dot/shape
    this.ctx.beginPath();
    if (marker.type === 'milestone') {
      const size = config.dotSize;
      this.ctx.moveTo(x, config.dotY - size);
      this.ctx.lineTo(x + size, config.dotY);
      this.ctx.lineTo(x, config.dotY + size);
      this.ctx.lineTo(x - size, config.dotY);
      this.ctx.closePath();
    } else {
      this.ctx.arc(x, config.dotY, config.dotSize, 0, 2 * Math.PI);
    }
    this.ctx.fillStyle = marker.color;
    this.ctx.fill();

    if (size === 'large') {
      this.ctx.strokeStyle = '#fff';
      this.ctx.lineWidth = 2;
      this.ctx.stroke();
    }

    this.renderMarkerLabel(x, marker.label, marker.type, config);
  }

  private getMarkerConfig(size: 'small' | 'medium' | 'large', type: string) {
    const configs = {
      small: {
        dotSize: 3,
        dotY: 70,
        lineStart: 65,
        lineEnd: 75,
        lineWidth: 1,
        labelY: 85
      },
      medium: {
        dotSize: 4,
        dotY: 50,
        lineStart: 40,
        lineEnd: 60,
        lineWidth: 2,
        labelY: 35
      },
      large: {
        dotSize: 6,
        dotY: 50,
        lineStart: 20,
        lineEnd: 80,
        lineWidth: 3,
        labelY: 15
      }
    };
    
    return configs[size];
  }

  private renderMarkerLabel(x: number, label: string, type: string, config: any): void {
    this.ctx.fillStyle = '#495057';
    
    if (type === 'milestone') {
      this.ctx.font = '8px Arial';
    } else if (type === 'sequence_start' || type === 'sequence_end') {
      this.ctx.font = 'bold 10px Arial';
    } else {
      this.ctx.font = '9px Arial';
    }
    
    this.ctx.textAlign = 'center';
    
    const maxLength = type === 'milestone' ? 15 : 25;
    const displayLabel = label.length > maxLength ? label.substring(0, maxLength) + '...' : label;
    
    if (type === 'hole_blast' && label.includes('Wave')) {
      const textWidth = this.ctx.measureText(displayLabel).width;
      this.ctx.fillStyle = 'rgba(255, 255, 255, 0.8)';
      this.ctx.fillRect(x - textWidth/2 - 2, config.labelY - 8, textWidth + 4, 10);
      this.ctx.fillStyle = '#495057';
    }
    
    this.ctx.fillText(displayLabel, x, config.labelY);
  }

  private renderCurrentTimeIndicator(): void {
    const x = (this.currentTime / this.totalDuration) * 780 + 10;

    this.ctx.beginPath();
    this.ctx.moveTo(x, 10);
    this.ctx.lineTo(x, 90);
    this.ctx.strokeStyle = '#dc3545';
    this.ctx.lineWidth = 3;
    this.ctx.stroke();

    this.ctx.beginPath();
    this.ctx.arc(x, 50, 6, 0, 2 * Math.PI);
    this.ctx.fillStyle = '#dc3545';
    this.ctx.fill();
  }
} 