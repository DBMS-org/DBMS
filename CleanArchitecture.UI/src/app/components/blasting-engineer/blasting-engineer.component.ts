import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { BlastingEngineerLayoutComponent } from './shared/blasting-engineer-layout/blasting-engineer-layout.component';

@Component({
  selector: 'app-blasting-engineer',
  standalone: true,
  imports: [RouterOutlet, BlastingEngineerLayoutComponent],
  template: '<app-blasting-engineer-layout></app-blasting-engineer-layout>',
  styleUrl: './blasting-engineer.component.scss'
})
export class BlastingEngineerComponent {
}
