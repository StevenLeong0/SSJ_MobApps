
import React from 'react';
import { View, Text, Button, TextInput } from 'react-native';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { RootStackParamList, IItem } from './types';
import {useState} from 'react';
import {ItemContext} from './context';
import {ItemContextType} from './types';
import {useContext} from 'react';

type AddScreenProps = NativeStackScreenProps<RootStackParamList, 'Add'>;



export default function AddScreen({ navigation }: AddScreenProps) {

    const context = useContext(ItemContext);
    

    


    
    //  const [id, setId] = useState('');
    const [title, setTitle] = useState('');

const handleSubmit = () => {
  
  if (title.trim()) {
    

    const newBulletin = 
{id: Date.now().toString(),
    title: title.trim()};


if (!context){
    alert ('Context not avaialable')
    return;
}

const {saveBulletins} = context;

saveBulletins(newBulletin);




    navigation.navigate('MemberBulletinSummary');
  } else {
    alert('Please fill out all fields.');
  }
};


  return (



    
    <View>
      <Text>Add Screen</Text>

{/*  <TextInput
        
        placeholder="Enter id"
        onChangeText={newText => setId(newText)}
      
      />
 */}
 <TextInput
        
        placeholder="Enter title"
        onChangeText={newText => setTitle(newText)}
      
      />

      <Button
        title="Submit"
        onPress= {handleSubmit}
      />

  

    </View>
  );
}