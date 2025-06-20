import { Component } from '@angular/core';
import { StoreManagerLayoutComponent } from './shared/store-manager-layout/store-manager-layout.component';

@Component({
  selector: 'app-store-manager',
  standalone: true,
  imports: [StoreManagerLayoutComponent],
  template: '<app-store-manager-layout></app-store-manager-layout>',
  styleUrl: './store-manager.component.scss'
})
export class StoreManagerComponent {
}
