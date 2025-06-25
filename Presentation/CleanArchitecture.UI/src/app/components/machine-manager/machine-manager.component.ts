import { Component } from '@angular/core';
import { MachineManagerLayoutComponent } from './shared/machine-manager-layout/machine-manager-layout.component';

@Component({
  selector: 'app-machine-manager',
  standalone: true,
  imports: [MachineManagerLayoutComponent],
  template: '<app-machine-manager-layout></app-machine-manager-layout>',
  styleUrl: './machine-manager.component.scss'
})
export class MachineManagerComponent {
}
