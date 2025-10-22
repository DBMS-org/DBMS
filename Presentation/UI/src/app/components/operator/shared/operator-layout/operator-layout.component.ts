import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { OperatorNavbarComponent } from '../navbar/navbar.component';
import { OperatorSidebarComponent } from '../sidebar/sidebar.component';

// Main layout component for the operator module that handles navbar, sidebar, and routing
@Component({
  selector: 'app-operator-layout',
  standalone: true,
  imports: [RouterOutlet, OperatorNavbarComponent, OperatorSidebarComponent],
  templateUrl: './operator-layout.component.html',
  styleUrl: './operator-layout.component.scss'
})
export class OperatorLayoutComponent {
  // Track sidebar collapse state for responsive layout
  isSidebarCollapsed = false;

  // Toggle sidebar visibility between expanded and collapsed states
  toggleSidebar() {
    this.isSidebarCollapsed = !this.isSidebarCollapsed;
  }
}
