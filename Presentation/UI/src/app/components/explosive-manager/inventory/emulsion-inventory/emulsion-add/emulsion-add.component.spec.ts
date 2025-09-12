import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmulsionAddComponent } from './emulsion-add.component';

describe('EmulsionAddComponent', () => {
  let component: EmulsionAddComponent;
  let fixture: ComponentFixture<EmulsionAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EmulsionAddComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EmulsionAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
