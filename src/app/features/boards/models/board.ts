export interface Board {
  id: string;
  name: string;
  description: string;
  ownerUserId: string;
  isPublic: boolean;
  isPremium: boolean;
  createdAt: string;
}
