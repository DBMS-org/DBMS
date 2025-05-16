import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DrillingPatternCreatorComponent } from './drilling-pattern-creator.component';

describe('DrillingPatternCreatorComponent', () => {
  let component: DrillingPatternCreatorComponent;
  let fixture: ComponentFixture<DrillingPatternCreatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DrillingPatternCreatorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DrillingPatternCreatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
