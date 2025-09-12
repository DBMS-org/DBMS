import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StoreStatistics } from '../../../../core/models/store.model';

@Component({
  selector: 'app-store-statistics',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './store-statistics.component.html',
  styleUrl: './store-statistics.component.scss'
})
export class StoreStatisticsComponent {
  @Input() statistics: StoreStatistics | null = null;
}
