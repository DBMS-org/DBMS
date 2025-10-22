import { Component } from '@angular/core';
import { AdminLayoutComponent } from './shared/admin-layout/admin-layout.component';

// Root component for the admin module
@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [AdminLayoutComponent],
  template: '<app-admin-layout></app-admin-layout>',
  styleUrl: './admin.component.scss'
})
export class AdminComponent {
}
