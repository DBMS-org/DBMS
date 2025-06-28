import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { DashboardHomeComponent } from './dashboard-home/dashboard-home.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    DashboardHomeComponent
  ],
  template: `
      <app-dashboard-home></app-dashboard-home>
  `,
  styles: []
})
export class DashboardComponent {
}
