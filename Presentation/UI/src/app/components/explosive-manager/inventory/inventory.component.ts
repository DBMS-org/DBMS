import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { InventoryOverviewComponent } from './inventory-overview/inventory-overview.component';

@Component({
  selector: 'app-inventory',
  standalone: true,
  imports: [
    CommonModule,
    InventoryOverviewComponent
  ],
  templateUrl: './inventory.component.html',
  styleUrl: './inventory.component.scss'
})
export class InventoryComponent {
  currentView: 'overview' | 'anfo' | 'emulsion' = 'overview';

  constructor(private router: Router) {}

  setCurrentView(view: 'overview' | 'anfo' | 'emulsion'): void {
    this.currentView = view;
    
    if (view === 'anfo') {
      this.router.navigate(['/explosive-manager/inventory/anfo']);
    } else if (view === 'emulsion') {
      this.router.navigate(['/explosive-manager/inventory/emulsion']);
    }
  }
}
