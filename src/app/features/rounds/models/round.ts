export enum RoundStatus {
  Draft = 0,
  Open = 1,
  Active = 2,
  Completed = 3,
}

export interface Round {
  id: string;
  boardId: string;
  name: string;
  status: RoundStatus;
  startDateTime: string;
  endDateTime: string;
  createdAt: string;
}
