import { Component, inject, signal, computed, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router, NavigationEnd } from '@angular/router';
import { filter, map, startWith } from 'rxjs/operators';

interface BreadcrumbItem {
  label: string;
  route?: string;
  isActive: boolean;
}

@Component({
  selector: 'app-breadcrumb',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <nav class="breadcrumb-nav" role="navigation" aria-label="Breadcrumb">
      <ol class="breadcrumb-list">
        @for (item of breadcrumbItems(); track item.label) {
          <li class="breadcrumb-item" [class.active]="item.isActive">
            @if (item.route && !item.isActive) {
              <a [routerLink]="item.route" class="breadcrumb-link">
                {{ item.label }}
              </a>
            } @else {
              <span class="breadcrumb-text" [attr.aria-current]="item.isActive ? 'page' : null">
                {{ item.label }}
              </span>
            }
            @if (!item.isActive) {
              <i class="material-icons breadcrumb-separator" aria-hidden="true">chevron_right</i>
            }
          </li>
        }
      </ol>
    </nav>
  `,
  styleUrl: './breadcrumb.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BreadcrumbComponent {
  private router = inject(Router);
  
  private currentUrl = signal('');
  
  breadcrumbItems = computed(() => {
    const url = this.currentUrl();
    const items: BreadcrumbItem[] = [];
    
    // Always start with Mechanical Engineer
    items.push({
      label: 'Mechanical Engineer',
      route: '/mechanical-engineer/dashboard',
      isActive: false
    });
    
    if (url.includes('/maintenance')) {
      items.push({
        label: 'Maintenance',
        route: '/mechanical-engineer/maintenance',
        isActive: false
      });
      
      // Add specific maintenance section
      if (url.includes('/maintenance/jobs')) {
        items.push({
          label: 'Jobs',
          isActive: true
        });
      } else if (url.includes('/maintenance/analytics')) {
        items.push({
          label: 'Analytics',
          isActive: true
        });
      } else if (url.includes('/maintenance/settings')) {
        items.push({
          label: 'Settings',
          isActive: true
        });
      } else {
        // Default to jobs if just /maintenance
        items.push({
          label: 'Jobs',
          isActive: true
        });
      }
    }
    
    return items;
  });
  
  constructor() {
    // Listen to route changes
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      map(event => (event as NavigationEnd).url),
      startWith(this.router.url)
    ).subscribe(url => {
      this.currentUrl.set(url);
    });
  }
}