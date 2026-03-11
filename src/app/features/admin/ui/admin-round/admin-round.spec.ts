import { TestBed } from '@angular/core/testing';
import { AdminRoundComponent } from './admin-round';

describe('AdminRoundComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminRoundComponent],
    }).compileComponents();
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(AdminRoundComponent);
    fixture.componentRef.setInput('boardId', 'board-1');
    fixture.componentRef.setInput('roundId', 'round-1');
    expect(fixture.componentInstance).toBeTruthy();
  });
});
