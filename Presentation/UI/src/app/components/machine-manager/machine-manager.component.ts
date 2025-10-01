import { Component } from '@angular/core';
import { MachineManagerLayoutComponent } from './shared/machine-manager-layout/machine-manager-layout.component';

/**
 * Machine Manager Component
 * 
 * Root component for the machine manager module that provides a dedicated interface
 * for managing drilling machines, maintenance schedules, and operational assignments.
 * This component serves as the entry point for machine managers to access all
 * machine-related functionality within the system.
 */
@Component({
  selector: 'app-machine-manager',
  standalone: true,
  imports: [MachineManagerLayoutComponent],
  template: '<app-machine-manager-layout></app-machine-manager-layout>',
  styleUrl: './machine-manager.component.scss'
})
export class MachineManagerComponent {
}
