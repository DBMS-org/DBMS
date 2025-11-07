import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { OperatorNavbarComponent } from '../navbar/navbar.component';
import { OperatorSidebarComponent } from '../sidebar/sidebar.component';

@Component({
  selector: 'app-operator-layout',
  standalone: true,
  imports: [RouterOutlet, OperatorNavbarComponent, OperatorSidebarComponent],
  templateUrl: './operator-layout.component.html',
  styleUrl: './operator-layout.component.scss'
})
export class OperatorLayoutComponent {
  isSidebarCollapsed = false;

  toggleSidebar() {
    this.isSidebarCollapsed = !this.isSidebarCollapsed;
  }
}
