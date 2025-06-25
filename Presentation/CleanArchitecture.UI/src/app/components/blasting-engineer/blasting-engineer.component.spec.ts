import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BlastingEngineerComponent } from './blasting-engineer.component';

describe('BlastingEngineerComponent', () => {
  let component: BlastingEngineerComponent;
  let fixture: ComponentFixture<BlastingEngineerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BlastingEngineerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BlastingEngineerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
