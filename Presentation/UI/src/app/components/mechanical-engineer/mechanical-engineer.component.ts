import { Component } from '@angular/core';
import { MechanicalEngineerLayoutComponent } from './shared/mechanical-engineer-layout/mechanical-engineer-layout.component';

@Component({
  selector: 'app-mechanical-engineer',
  standalone: true,
  imports: [MechanicalEngineerLayoutComponent],
  template: '<app-mechanical-engineer-layout></app-mechanical-engineer-layout>',
  styleUrl: './mechanical-engineer.component.scss'
})
export class MechanicalEngineerComponent {
}
