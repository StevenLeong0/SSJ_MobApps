export type RootStackParamList = {
  Atrium: undefined;
  BulletinChoice: { username: string; password: string };
  Login: undefined;
  MemberBulletinSummary: undefined;
  Register: undefined;
  Edit: {item: IItem};
  MemberBulletinDetails: {
    item: { id: string; title: string };


  };

  Add: undefined;
 
};

export interface IItem {
  id: string;
  title: string;
}

export type ItemContextType = {
  bulletins: IItem[];
  saveBulletins: (item: IItem) => void;
  deleteBulletin: (id: string) => void;
  
  
}