import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AdminLayoutComponent } from '../shared/admin-layout/admin-layout.component';
import { DashboardHomeComponent } from './dashboard-home/dashboard-home.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    AdminLayoutComponent,
    DashboardHomeComponent
  ],
  template: `
    <app-admin-layout>
      <app-dashboard-home></app-dashboard-home>
    </app-admin-layout>
  `,
  styles: []
})
export class DashboardComponent {
}
