import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from '../navbar/navbar.component';
import { SidebarComponent } from '../sidebar/sidebar.component';

// Main layout component for machine manager interface with navbar, sidebar, and routing
@Component({
  selector: 'app-machine-manager-layout',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent, SidebarComponent],
  templateUrl: './machine-manager-layout.component.html',
  styleUrl: './machine-manager-layout.component.scss'
})
export class MachineManagerLayoutComponent {
  // Track sidebar collapse state
  isSidebarCollapsed = false;

  // Toggle sidebar between collapsed and expanded states
  toggleSidebar() {
    this.isSidebarCollapsed = !this.isSidebarCollapsed;
  }
}
