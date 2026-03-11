import { TestBed } from '@angular/core/testing';
import { AdminBoardComponent } from './admin-board';

describe('AdminBoardComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminBoardComponent],
    }).compileComponents();
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(AdminBoardComponent);
    fixture.componentRef.setInput('boardId', 'board-1');
    expect(fixture.componentInstance).toBeTruthy();
  });
});
