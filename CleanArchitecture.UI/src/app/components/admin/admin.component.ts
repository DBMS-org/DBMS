import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AdminLayoutComponent } from './shared/admin-layout/admin-layout.component';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [RouterOutlet, AdminLayoutComponent],
  template: '<app-admin-layout></app-admin-layout>',
  styleUrl: './admin.component.scss'
})
export class AdminComponent {
}
