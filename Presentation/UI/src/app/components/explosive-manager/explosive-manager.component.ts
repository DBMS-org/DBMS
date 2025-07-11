import { Component } from '@angular/core';
import { ExplosiveManagerLayoutComponent } from './shared/explosive-manager-layout/explosive-manager-layout.component';

@Component({
  selector: 'app-explosive-manager',
  standalone: true,
  imports: [ExplosiveManagerLayoutComponent],
  template: '<app-explosive-manager-layout></app-explosive-manager-layout>',
  styleUrl: './explosive-manager.component.scss'
})
export class ExplosiveManagerComponent {
}
