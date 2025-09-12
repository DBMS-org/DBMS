import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-emulsion-inventory',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './emulsion-inventory.component.html',
  styleUrl: './emulsion-inventory.component.scss'
})
export class EmulsionInventoryComponent {

  constructor(private router: Router) {}

  navigateToAdd(): void {
    this.router.navigate(['/explosive-manager/inventory/emulsion/add']);
  }

  navigateToEdit(batchId: string): void {
    this.router.navigate(['/explosive-manager/inventory/emulsion/edit', batchId]);
  }
}
