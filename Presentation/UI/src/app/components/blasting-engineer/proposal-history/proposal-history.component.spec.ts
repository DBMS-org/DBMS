import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProposalHistoryComponent } from './proposal-history.component';

describe('ProposalHistoryComponent', () => {
  let component: ProposalHistoryComponent;
  let fixture: ComponentFixture<ProposalHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProposalHistoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProposalHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
