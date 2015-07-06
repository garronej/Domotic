#!/bin/bash


cat db_reset.sql > restore.sql

week=$(( 6*24*7 ))

out="\nGO"


prev=21
i=1
while [ $i -le $week ]
do
  prev=$(($prev>12?$prev:12))
  prev=$(($prev<27?$prev:27))
  prev=$(( ( RANDOM % 3 )  +  $prev - 1 ))


  out="$out\nINSERT [dbo].temperature ([time], [value]"


  n=$(($i * 10))

  date=$(date +"%C%y-%m-%d %H:%M:%S:000" -d "now - $n min")


  out="$out\n) VALUES (convert(char(22),'$date',121),$prev)"


  i=$(( $i + 1 ))
done



echo -e $out >> restore.sql
