import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from '../navbar/navbar.component';
import { SidebarComponent } from '../sidebar/sidebar.component';

/**
 * Machine Manager Layout Component
 * 
 * Provides the main layout structure for the machine manager interface, including
 * navigation components and content area. Manages the sidebar collapse/expand
 * functionality to optimize screen space usage for different viewing preferences.
 */
@Component({
  selector: 'app-machine-manager-layout',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent, SidebarComponent],
  templateUrl: './machine-manager-layout.component.html',
  styleUrl: './machine-manager-layout.component.scss'
})
export class MachineManagerLayoutComponent {
  // Controls the collapsed/expanded state of the sidebar navigation
  isSidebarCollapsed = false;

  /**
   * Toggles the sidebar between collapsed and expanded states
   * Used to provide more screen real estate when needed
   */
  toggleSidebar() {
    this.isSidebarCollapsed = !this.isSidebarCollapsed;
  }
}
