import { Component, OnInit, AfterViewInit, ElementRef, Renderer2 } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { BlastSequenceSimulatorComponent } from '../../../../components/blasting-engineer/blast-sequence-simulator/blast-sequence-simulator.component';

@Component({
  selector: 'app-view-sequence-simulator',
  standalone: true,
  imports: [CommonModule, BlastSequenceSimulatorComponent],
  templateUrl: './view-sequence-simulator.component.html',
  styleUrls: ['./view-sequence-simulator.component.scss']
})
export class ViewSequenceSimulatorComponent implements OnInit, AfterViewInit {
  projectId = 0;
  siteId = 0;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private elementRef: ElementRef,
    private renderer: Renderer2
  ) {}

  ngOnInit(): void {
    // Extract route parameters
    this.route.paramMap.subscribe(params => {
      this.projectId = +(params.get('projectId') || '0');
      this.siteId = +(params.get('siteId') || '0');
      
      console.log('View Sequence Simulator - Project ID:', this.projectId, 'Site ID:', this.siteId);
    });
  }

  ngAfterViewInit(): void {
    // Wait for the blast sequence simulator to load, then hide the save button
    setTimeout(() => {
      this.hideSaveButton();
    }, 1000);
  }

  /**
   * Programmatically hide the save button from the blast sequence simulator
   */
  private hideSaveButton(): void {
    const saveButtons = this.elementRef.nativeElement.querySelectorAll(
      '.action-button.save-button, .header-actions .save-button, .header-actions .action-button:first-child'
    );
    
    saveButtons.forEach((button: HTMLElement) => {
      this.renderer.setStyle(button, 'display', 'none');
      this.renderer.setStyle(button, 'visibility', 'hidden');
      this.renderer.setStyle(button, 'opacity', '0');
      this.renderer.setStyle(button, 'pointer-events', 'none');
      console.log('Admin view: Save button hidden programmatically');
    });

    // Also try to hide by text content
    const allButtons = this.elementRef.nativeElement.querySelectorAll('button');
    allButtons.forEach((button: HTMLElement) => {
      const buttonText = button.textContent?.toLowerCase();
      const buttonTitle = button.getAttribute('title')?.toLowerCase();
      
      if (buttonText?.includes('save') || buttonTitle?.includes('save')) {
        this.renderer.setStyle(button, 'display', 'none');
        this.renderer.setStyle(button, 'visibility', 'hidden');
        this.renderer.setStyle(button, 'opacity', '0');
        this.renderer.setStyle(button, 'pointer-events', 'none');
        console.log('Admin view: Save button hidden by text content');
      }
    });
  }

  /**
   * Navigate back to the sites management page
   */
  goBack(): void {
    console.log('Navigating back to sites from admin simulator...');
    
    const firstSegment = this.router.url.split('/')[1]; // 'admin' or 'blasting-engineer'

    if (this.projectId) {
      this.router.navigate([`/${firstSegment}/project-management`, this.projectId, 'sites']);
    } else {
      this.router.navigate([`/${firstSegment}/project-management`]);
    }
  }
} 