import React from 'react';
import { View, Text, Button } from 'react-native';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { RootStackParamList } from './types';
import {ItemContext} from './context';
import {ItemContextType, IItem} from './types';
import {useContext} from 'react';



type MemberBulletinDetailsScreenProps = NativeStackScreenProps<RootStackParamList, 'MemberBulletinDetails'>;

export default function MemberBulletinDetailsScreen({ navigation, route }: MemberBulletinDetailsScreenProps) {
;

const context = useContext(ItemContext);

if (!context){
  return <Text> Loading....</Text>
}
const {bulletins, deleteBulletin} = context;
const {item} = route.params;

 const deleteItem = (idToDelete: string) => {
 deleteBulletin(idToDelete)
  navigation.navigate("MemberBulletinSummary");
 }


 
 
    return (
    <View>
      <Text>Member bulletin details</Text>

    <Text>{item.id}</Text>
    <Text> {item.title}</Text>

      <Button
        title="Edit"
        onPress={() => navigation.navigate('Edit', {item})}
      />

      <Button
      title = "Delete"
      onPress= {()=> {deleteItem(item.id)}}/>




    </View>
  );
}
