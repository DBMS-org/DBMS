import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DrillVisualizationComponent } from './drill-visualization.component';

describe('DrillVisualizationComponent', () => {
  let component: DrillVisualizationComponent;
  let fixture: ComponentFixture<DrillVisualizationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DrillVisualizationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DrillVisualizationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
