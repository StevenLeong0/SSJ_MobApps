import React, {useState, useEffect} from 'react';
import { FlatList, Text, View, TouchableOpacity, Button } from 'react-native';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { RootStackParamList } from './types';
import {useContext} from 'react';
import {ItemContext} from './context';
import {IItem, ItemContextType} from './types';

type Props = NativeStackScreenProps<
  RootStackParamList,
  'MemberBulletinSummary'
>;


const MemberBulletinSummary: React.FC<Props> = ({ navigation }) => {

const context = useContext (ItemContext);



if (!context){
  return <Text> Loading ...</Text>
}

const {bulletins} = context;

  const renderItem = ({
    item,
  }: {
    item: { id: string; title: string };
  }) => (
    <TouchableOpacity
      onPress={() =>
        navigation.navigate('MemberBulletinDetails', { item})
      }
    >
      <View>
        <Text>{item.title}</Text>
      </View>
    </TouchableOpacity>
  );


  return (
    <View>
    <FlatList
      data={bulletins}
      renderItem={renderItem}
      keyExtractor={(item) => item.id}
    />

       <Button
        title="Add"
        onPress={() => navigation.navigate('Add')}
      />

</View>

  );
};




export default MemberBulletinSummary;
