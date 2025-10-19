import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from '../navbar/navbar.component';
import { SidebarComponent } from '../sidebar/sidebar.component';

// Main layout component for mechanical engineer interface with navbar, sidebar, and routing
@Component({
  selector: 'app-mechanical-engineer-layout',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent, SidebarComponent],
  templateUrl: './mechanical-engineer-layout.component.html',
  styleUrl: './mechanical-engineer-layout.component.scss'
})
export class MechanicalEngineerLayoutComponent {
  // Track sidebar collapse state
  isSidebarCollapsed = false;

  // Toggle sidebar between collapsed and expanded states
  toggleSidebar() {
    this.isSidebarCollapsed = !this.isSidebarCollapsed;
  }
}
