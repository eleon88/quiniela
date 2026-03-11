import { TestBed } from '@angular/core/testing';
import { LeaderboardComponent } from './leaderboard';

describe('LeaderboardComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LeaderboardComponent],
    }).compileComponents();
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(LeaderboardComponent);
    fixture.componentRef.setInput('boardId', 'board-1');
    fixture.componentRef.setInput('roundId', 'round-1');
    expect(fixture.componentInstance).toBeTruthy();
  });
});
