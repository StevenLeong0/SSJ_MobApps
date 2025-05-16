import React from 'react';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import AtriumScreen from './AtriumScreen';
import LoginScreen from './LoginScreen';
//import RegisterScreen from './RegisterScreen';
import BulletinChoiceScreen from './BulletinChoiceScreen';
import MemberBulletinSummaryScreen from './MemberBulletinSummaryScreen';
import MemberBulletinDetailsScreen from './MemberBulletinDetailsScreen';
import AddScreen from './AddScreen';
import EditScreen from './EditScreen';


import { RootStackParamList } from './types';

const Stack = createNativeStackNavigator<RootStackParamList>();
export default function StackNavigator() {
  return (
    <Stack.Navigator initialRouteName="Atrium">
      <Stack.Screen name="Atrium" component={AtriumScreen} />
      <Stack.Screen name="Login" component={LoginScreen} />


 {/* <Stack.Screen name="Register" component={RegisterScreen} /> */}
 

  <Stack.Screen name="BulletinChoice" component={BulletinChoiceScreen} />
   <Stack.Screen name="MemberBulletinSummary" component={MemberBulletinSummaryScreen} />
    <Stack.Screen name="MemberBulletinDetails" component={MemberBulletinDetailsScreen} />

     <Stack.Screen name="Add" component={AddScreen} />

       <Stack.Screen name="Edit" component={EditScreen} /> 

    </Stack.Navigator>
  );
}
