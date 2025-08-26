import { Component, input, computed, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ProblemCategory, MachinePart, PROBLEM_CATEGORY_OPTIONS, MACHINE_PART_OPTIONS } from '../../models/maintenance-report.models';

@Component({
  selector: 'app-problem-category-icons',
  imports: [
    CommonModule,
    MatIconModule,
    MatTooltipModule
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="icon-container">
      @if (problemCategory()) {
        <mat-icon 
          [matTooltip]="categoryTooltip()"
          class="category-icon">
          {{ categoryIcon() }}
        </mat-icon>
      }
      @if (machinePart()) {
        <mat-icon 
          [matTooltip]="partTooltip()"
          class="part-icon">
          {{ partIcon() }}
        </mat-icon>
      }
    </div>
  `,
  styles: [`
    .icon-container {
      display: flex;
      align-items: center;
      gap: 0.5rem;
    }

    .category-icon {
      color: #1976d2;
      font-size: 20px;
      width: 20px;
      height: 20px;
    }

    .part-icon {
      color: #666;
      font-size: 18px;
      width: 18px;
      height: 18px;
    }

    .icon-container mat-icon {
      cursor: help;
    }
  `]
})
export class ProblemCategoryIconsComponent {
  problemCategory = input<ProblemCategory>();
  machinePart = input<MachinePart>();

  categoryIcon = computed(() => {
    const category = this.problemCategory();
    if (!category) return '';
    
    const config = PROBLEM_CATEGORY_OPTIONS.find(option => option.value === category);
    return config?.icon || 'help_outline';
  });

  partIcon = computed(() => {
    const part = this.machinePart();
    if (!part) return '';
    
    const config = MACHINE_PART_OPTIONS.find(option => option.value === part);
    return config?.icon || 'help_outline';
  });

  categoryTooltip = computed(() => {
    const category = this.problemCategory();
    if (!category) return '';
    
    const config = PROBLEM_CATEGORY_OPTIONS.find(option => option.value === category);
    return `Problem Category: ${config?.label || category}`;
  });

  partTooltip = computed(() => {
    const part = this.machinePart();
    if (!part) return '';
    
    const config = MACHINE_PART_OPTIONS.find(option => option.value === part);
    return `Affected Part: ${config?.label || part}`;
  });
}