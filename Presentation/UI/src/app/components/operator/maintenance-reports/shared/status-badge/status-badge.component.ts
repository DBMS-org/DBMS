import { Component, input, computed, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ReportStatus, REPORT_STATUS_OPTIONS } from '../../models/maintenance-report.models';

@Component({
  selector: 'app-status-badge',
  imports: [
    CommonModule,
    MatChipsModule,
    MatIconModule,
    MatTooltipModule
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <mat-chip-set>
      <mat-chip 
        [style.background-color]="statusConfig().color"
        [style.color]="textColor()"
        [matTooltip]="tooltipText()"
        class="status-chip">
        <mat-icon matChipAvatar>{{ statusConfig().icon }}</mat-icon>
        {{ statusConfig().label }}
      </mat-chip>
    </mat-chip-set>
  `,
  styles: [`
    .status-chip {
      font-weight: 500;
      border: none;
      cursor: default;
    }

    .status-chip mat-icon {
      font-size: 16px;
      width: 16px;
      height: 16px;
    }
  `]
})
export class StatusBadgeComponent {
  status = input.required<ReportStatus>();
  showTooltip = input<boolean>(true);

  statusConfig = computed(() => {
    const config = REPORT_STATUS_OPTIONS.find(option => option.value === this.status());
    return config || REPORT_STATUS_OPTIONS[0]; // Default to first option if not found
  });

  textColor = computed(() => {
    const bgColor = this.statusConfig().color;
    // Simple contrast calculation - use white text for dark backgrounds
    const hex = bgColor.replace('#', '');
    const r = parseInt(hex.substr(0, 2), 16);
    const g = parseInt(hex.substr(2, 2), 16);
    const b = parseInt(hex.substr(4, 2), 16);
    const brightness = (r * 299 + g * 587 + b * 114) / 1000;
    return brightness > 128 ? '#000000' : '#ffffff';
  });

  tooltipText = computed(() => {
    if (!this.showTooltip()) return '';
    
    const status = this.status();
    switch (status) {
      case ReportStatus.REPORTED:
        return 'Report has been submitted and is awaiting review by mechanical engineers';
      case ReportStatus.ACKNOWLEDGED:
        return 'Report has been reviewed and acknowledged by a mechanical engineer';
      case ReportStatus.IN_PROGRESS:
        return 'Mechanical engineer is actively working on resolving the issue';
      case ReportStatus.RESOLVED:
        return 'Issue has been resolved by the mechanical engineer';
      case ReportStatus.CLOSED:
        return 'Report has been closed and archived';
      default:
        return this.statusConfig().label;
    }
  });
}