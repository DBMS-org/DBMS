import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { BlastSequenceSimulatorComponent } from '../../../blasting-engineer/blast-sequence-simulator/blast-sequence-simulator.component';

@Component({
  selector: 'app-view-sequence-simulator',
  standalone: true,
  imports: [CommonModule, BlastSequenceSimulatorComponent],
  template: `
    <app-blast-sequence-simulator></app-blast-sequence-simulator>
  `,
  styleUrls: [],
  styles: [
    `:host ::ng-deep .right-controls button[title='Save simulation'] { display: none !important; }`
  ]
})
export class ViewSequenceSimulatorComponent implements OnInit {
  projectId = 0;
  siteId = 0;

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.projectId = +(this.route.snapshot.paramMap.get('projectId') || '0');
    this.siteId = +(this.route.snapshot.paramMap.get('siteId') || '0');
    // TODO: pass these ids to simulator service if needed
  }
} 