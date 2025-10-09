import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

interface Notification {
  id: number;
  message: string;
  date: string;
}

@Component({
  selector: 'app-maintenance-notifications',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './maintenance-notifications.component.html',
  styleUrls: ['./maintenance-notifications.component.scss']
})
export class MaintenanceNotificationsComponent {
  notifications: Notification[] = [
    { id: 1, message: 'Maintenance due for Drill Machine #45', date: '2023-10-01' },
    { id: 2, message: 'Low oil level in Compressor #12', date: '2023-10-02' },
    { id: 3, message: 'Scheduled service for Truck #7 completed', date: '2023-10-03' }
  ]; // Mock data, replace with service call in real implementation
}