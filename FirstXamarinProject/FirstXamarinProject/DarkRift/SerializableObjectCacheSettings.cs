using DarkRift;
using DarkRift.Client;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FirstXamarinProject.DarkRift
{
    [Serializable]
    public sealed class SerializableObjectCacheSettings
    {
#pragma warning disable IDE0044 // Add readonly modifier, Unity can't serialize readonly fields
 
        private int maxWriters = 2;

 
        private int maxReaders = 2;

   
        private int maxMessages = 4;

   
        private int maxMessageBuffers = 4;


        private int maxSocketAsyncEventArgs = 32;


        private int maxActionDispatcherTasks = 16;

  
        private int maxAutoRecyclingArrays = 4;

      
        private int extraSmallMemoryBlockSize = 16;

   
        private int maxExtraSmallMemoryBlocks = 2;

     
        private int smallMemoryBlockSize = 64;

    
        private int maxSmallMemoryBlocks = 2;

    
        private int mediumMemoryBlockSize = 256;

 
        private int maxMediumMemoryBlocks = 2;

     
        private int largeMemoryBlockSize = 1024;

  
        private int maxLargeMemoryBlocks = 2;

    
        private int extraLargeMemoryBlockSize = 4096;

 
        private int maxExtraLargeMemoryBlocks = 2;


        private int maxMessageReceivedEventArgs = 4;
        #pragma warning restore IDE0044 // Add readonly modifier, Unity can't serialize readonly fields

        public ObjectCacheSettings ToClientObjectCacheSettings()
        {
            return new ObjectCacheSettings
            {
                MaxWriters = maxWriters,
                MaxReaders = maxReaders,
                MaxMessages = maxMessages,
                MaxMessageBuffers = maxMessageBuffers,
                MaxSocketAsyncEventArgs = maxSocketAsyncEventArgs,
                MaxActionDispatcherTasks = maxActionDispatcherTasks,
                MaxAutoRecyclingArrays = maxAutoRecyclingArrays,

                ExtraSmallMemoryBlockSize = extraSmallMemoryBlockSize,
                MaxExtraSmallMemoryBlocks = maxExtraSmallMemoryBlocks,
                SmallMemoryBlockSize = smallMemoryBlockSize,
                MaxSmallMemoryBlocks = maxSmallMemoryBlocks,
                MediumMemoryBlockSize = mediumMemoryBlockSize,
                MaxMediumMemoryBlocks = maxMediumMemoryBlocks,
                LargeMemoryBlockSize = largeMemoryBlockSize,
                MaxLargeMemoryBlocks = maxLargeMemoryBlocks,
                ExtraLargeMemoryBlockSize = extraLargeMemoryBlockSize,
                MaxExtraLargeMemoryBlocks = maxExtraLargeMemoryBlocks,

                //MaxMessageReceivedEventArgs = maxMessageReceivedEventArgs
            };
        }

        [Obsolete("Use ToClientObjectCacheSettings() in order to get the full set of settings.")]
        public ObjectCacheSettings ToObjectCacheSettings()
        {
            return ToClientObjectCacheSettings();
        }
    }

}
