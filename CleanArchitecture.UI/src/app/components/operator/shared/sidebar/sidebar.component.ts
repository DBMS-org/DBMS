import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

interface NavItem {
  icon: string;
  label: string;
  route: string;
}

@Component({
  selector: 'app-operator-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss'
})
export class OperatorSidebarComponent {
  @Input() isCollapsed = false;

  navItems: NavItem[] = [
    { icon: 'dashboard', label: 'Dashboard', route: '/operator/dashboard' },
    { icon: 'assignment', label: 'My Project', route: '/operator/my-project' }
  ];
} 