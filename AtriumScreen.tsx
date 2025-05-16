
import React from 'react';
import { View, Text, Button } from 'react-native';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { RootStackParamList } from './types';

type AtriumScreenProps = NativeStackScreenProps<RootStackParamList, 'Atrium'>;

export default function AtriumScreen({ navigation }: AtriumScreenProps) {
  return (
    <View>
      <Text>Home Screen</Text>
      <Button
        title="Go to Login"
        onPress={() => navigation.navigate('Login')}
      />
    </View>
  );
}
