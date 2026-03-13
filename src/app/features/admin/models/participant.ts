export interface Participant {
  id: string;
  roundId: string;
  userId: string | null;
  displayName: string;
  isActive: boolean;
  score: number;
  createdAt: string;
}
