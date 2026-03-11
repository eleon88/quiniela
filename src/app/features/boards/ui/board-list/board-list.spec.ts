import { TestBed } from '@angular/core/testing';
import { BoardListComponent } from './board-list';

describe('BoardListComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BoardListComponent],
    }).compileComponents();
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(BoardListComponent);
    expect(fixture.componentInstance).toBeTruthy();
  });
});
