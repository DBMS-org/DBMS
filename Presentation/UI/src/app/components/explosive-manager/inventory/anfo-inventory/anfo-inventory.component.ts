import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-anfo-inventory',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './anfo-inventory.component.html',
  styleUrl: './anfo-inventory.component.scss'
})
export class AnfoInventoryComponent {

  constructor(private router: Router) {}

  navigateToAdd(): void {
    this.router.navigate(['/explosive-manager/inventory/anfo/add']);
  }

  navigateToEdit(batchId: string): void {
    this.router.navigate(['/explosive-manager/inventory/anfo/edit', batchId]);
  }
}
