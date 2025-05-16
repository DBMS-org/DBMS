import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard-home.component.html',
  styleUrls: ['./dashboard-home.component.scss']
})
export class DashboardHomeComponent {
  stats = {
    totalUsers: 150,
    activeUsers: 120,
    totalOrders: 450,
    revenue: 25000
  };

  recentActivities = [
    { id: 1, user: 'John Doe', action: 'placed a new order', time: '5 minutes ago' },
    { id: 2, user: 'Jane Smith', action: 'updated their profile', time: '1 hour ago' },
    { id: 3, user: 'Bob Johnson', action: 'completed payment', time: '2 hours ago' },
    { id: 4, user: 'Alice Brown', action: 'requested support', time: '3 hours ago' }
  ];
}
