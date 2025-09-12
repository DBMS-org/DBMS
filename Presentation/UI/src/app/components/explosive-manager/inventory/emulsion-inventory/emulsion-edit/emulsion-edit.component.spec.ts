import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmulsionEditComponent } from './emulsion-edit.component';

describe('EmulsionEditComponent', () => {
  let component: EmulsionEditComponent;
  let fixture: ComponentFixture<EmulsionEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EmulsionEditComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EmulsionEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
