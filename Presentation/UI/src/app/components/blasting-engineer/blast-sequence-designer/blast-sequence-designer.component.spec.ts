import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BlastSequenceDesignerComponent } from './blast-sequence-designer.component';

describe('BlastSequenceDesignerComponent', () => {
  let component: BlastSequenceDesignerComponent;
  let fixture: ComponentFixture<BlastSequenceDesignerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BlastSequenceDesignerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BlastSequenceDesignerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
