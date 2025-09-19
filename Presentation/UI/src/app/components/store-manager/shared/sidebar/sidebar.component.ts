import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

interface NavItem {
  icon: string;
  label: string;
  route: string;
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss'
})
export class SidebarComponent {
  @Input() isCollapsed = false;

  navItems: NavItem[] = [
    { icon: 'dashboard', label: 'Dashboard', route: '/store-manager/dashboard' },
    { icon: 'add_box', label: 'Add Stock Request', route: '/store-manager/add-stock' },
    { icon: 'history', label: 'Request History', route: '/store-manager/request-history' },
    { icon: 'engineering', label: 'Blasting Engineer Requests', route: '/store-manager/blasting-engineer-requests' },
    { icon: 'local_shipping', label: 'Dispatch Preparation', route: '/store-manager/dispatch' }
  ];
}
