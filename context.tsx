import * as React from 'react';
import { ItemContextType, IItem } from './types';

export const ItemContext = React.createContext<ItemContextType | null>(null);

const Provider: React.FC<{children: React.ReactNode}> = ({ children }) => {
  const [bulletins, setBulletins] = React.useState<IItem[]>([
  { id: '1', title: 'First Item' },
  { id: '2', title: 'Second Item' },
  { id: '3', title: 'Third Item' },
]);
  

  // const saveBulletins = (item: IItem) => {
  //   const newItem: IItem = {
  //     id: item.id,
  //     title: item.title
      
  //   };
  //   setBulletins(prevBulletins => [...prevBulletins, newItem]);


  // };



const saveBulletins = (newBulletin:IItem) =>
{

  setBulletins ((prevBulletins)=> {
    const exists = prevBulletins.some(b=>b.id ===newBulletin.id);


    if (exists){
      return prevBulletins.map(b=>
        b.id === newBulletin.id? newBulletin: b);
      
    }
    else
    {return [...prevBulletins, newBulletin];}
  });
}

  const deleteBulletin = (idToDelete: string) =>{
    setBulletins (prev => prev.filter(item => item.id !== idToDelete));
  }


    return <ItemContext.Provider value={{bulletins, saveBulletins, deleteBulletin}}>{children}</ItemContext.Provider>;
};

export default Provider;

  