global using Thread = Forum_System.Models.Thread;
using Forum_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum_System.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {}

        public DbSet<User> Users { get; set; }
        public DbSet<Thread> Threads { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Tag> Tags { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Pw is 'securepass'
            List<User> users = new List<User>()
            {
                new User { Id = 1, FirstName = "John", LastName = "Doer", Username = "johndoe", Password = "c2VjdXJlcGFzcw==", Email = "john@example.com" },
                new User { Id = 2, FirstName = "Jane", LastName = "Smith", Username = "janesmith", Password = "c2VjdXJlcGFzcw==", Email = "jane@example.com" },
                new User { Id = 3, FirstName = "Bobby", LastName = "Fisher", Username = "bobjohnson", Password = "c2VjdXJlcGFzcw==", Email = "bob@example.com" },
                new User { Id = 4, FirstName = "Alice", LastName = "Williams", Username = "alicewill", Password = "c2VjdXJlcGFzcw==", Email = "alice@example.com" },
                new User { Id = 5, FirstName = "Charlie", LastName = "Brown", Username = "charlieb", Password = "c2VjdXJlcGFzcw==", Email = "charlie@example.com" },
                new User { Id = 6, FirstName = "Eva", LastName = "Miller", Username = "evam", Password = "c2VjdXJlcGFzcw==", Email = "eva@example.com" },
                new User { Id = 7, FirstName = "David", LastName = "Davis", Username = "davidd", Password = "c2VjdXJlcGFzcw==", Email = "david@example.com" },
                new User { Id = 8, FirstName = "Sophia", LastName = "Leeyam", Username = "sophialee", Password = "c2VjdXJlcGFzcw==", Email = "sophia@example.com" },
                new User { Id = 9, FirstName = "Michael", LastName = "White", Username = "michaelw", Password = "c2VjdXJlcGFzcw==", Email = "michael@example.com" },
                new User { Id = 10, FirstName = "Emma", LastName = "Clark", Username = "emmaclark", Password = "c2VjdXJlcGFzcw==", Email = "emma@example.com" },
                new User { Id = 11, FirstName = "Admin", LastName = "Admin", Username = "admin", Password = "c2VjdXJlcGFzcw==", Email = "admin@example.com", IsAdmin = true },
                new User { Id = 12, FirstName = "Grace", LastName = "Johnson", Username = "gracej", Password = "c2VjdXJlcGFzcw==", Email = "grace@example.com" },
                new User { Id = 13, FirstName = "Henry", LastName = "Anderson", Username = "henrya", Password = "c2VjdXJlcGFzcw==", Email = "henry@example.com" },
                new User { Id = 14, FirstName = "Olivia", LastName = "Smithson", Username = "olivias", Password = "c2VjdXJlcGFzcw==", Email = "olivia@example.com" },
                new User { Id = 15, FirstName = "Daniel", LastName = "Miller", Username = "danm", Password = "c2VjdXJlcGFzcw==", Email = "daniel@example.com" },
                new User { Id = 16, FirstName = "Lily", LastName = "Thompson", Username = "lilyt", Password = "c2VjdXJlcGFzcw==", Email = "lily@example.com" },
                new User { Id = 17, FirstName = "James", LastName = "Wong", Username = "jamesw", Password = "c2VjdXJlcGFzcw==", Email = "james@example.com" },
                new User { Id = 18, FirstName = "Isabella", LastName = "Lee", Username = "isabellalee", Password = "c2VjdXJlcGFzcw==", Email = "isabella@example.com" },
                new User { Id = 19, FirstName = "Oliver", LastName = "Taylor", Username = "olivert", Password = "c2VjdXJlcGFzcw==", Email = "oliver@example.com" },
                new User { Id = 20, FirstName = "Mia", LastName = "Garcia", Username = "miag", Password = "c2VjdXJlcGFzcw==", Email = "mia@example.com" },
                new User { Id = 21, FirstName = "Ethan", LastName = "Brown", Username = "ethanb", Password = "c2VjdXJlcGFzcw==", Email = "ethan@example.com" },
            };
            modelBuilder.Entity<User>().HasData(users);

            //fluentAPI begin

            modelBuilder.Entity<Rating>()
                .HasKey(r => new { r.ThreadId, r.UserId });

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Thread)
                .WithMany(b => b.Ratings)
                .HasForeignKey(r => r.ThreadId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Comment>()
               .HasKey(c => c.Id);  // Use 'Id' as the primary key

            modelBuilder.Entity<Comment>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();  // Let the database generate values for 'Id'

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Thread)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.ThreadId)
                .OnDelete(DeleteBehavior.Cascade);

            //fluentAPI end

            List<Thread> threads = new List<Thread>()
            {
                new Thread { Id = 1, AuthorId = 1, Title = "Favorite RPGs in 2023", Content = "What are your top three favorite RPGs and why? Share your experiences and recommendations." },
                new Thread { Id = 2, AuthorId = 2, Title = "Gaming Setup Showcase", Content = "Let's see your gaming setups! Post pictures and describe the key elements of your gaming rig." },
                new Thread { Id = 3, AuthorId = 3, Title = "Latest Gaming News", Content = "Discuss the latest gaming news and releases. What are you most excited about in the gaming world right now?" },
                new Thread { Id = 4, AuthorId = 4, Title = "Game Development Tips", Content = "For those interested in game development, share and discuss tips, tricks, and challenges you've encountered." },
                new Thread { Id = 5, AuthorId = 5, Title = "Virtual Reality Experiences", Content = "Have you tried any cool virtual reality games or experiences lately? Share your thoughts and recommendations." },
                new Thread { Id = 6, AuthorId = 6, Title = "Gaming Memories from Tomorrow", Content = "Share a memorable gaming experience from your past. What game left a lasting impression on you?" },
                new Thread { Id = 7, AuthorId = 7, Title = "Indie Game Spotlight", Content = "Discover and discuss hidden gems in the world of indie games. What indie titles have you recently enjoyed?" },
                new Thread { Id = 8, AuthorId = 8, Title = "Upcoming Game Releases", Content = "Stay updated on upcoming game releases. What games are you looking forward to playing in the near future?" },
                new Thread { Id = 9, AuthorId = 9, Title = "Favorite Gaming Quotes", Content = "Share your favorite quotes from video games. Whether it's inspirational or humorous, let's hear them!" },
                new Thread { Id = 10, AuthorId = 10, Title = "Game Recommendations", Content = "Looking for a new game to play? Ask for recommendations or share your favorite games with others." },
                new Thread { Id = 11, AuthorId = 11, Title = "Best Multiplayer Games of the Decade", Content = "Share your favorite multiplayer games that defined the past decade. From intense shooters to cooperative adventures, let's celebrate the best!" },
                new Thread { Id = 12, AuthorId = 13, Title = "Tech Enthusiasts Corner", Content = "Discuss the latest advancements in technology. From gadgets to software, share your thoughts on the tech trends shaping the future." },
                new Thread { Id = 13, AuthorId = 15, Title = "Bookworms Unite", Content = "For all the book lovers out there, recommend your favorite reads and discuss literary adventures. What book has left a lasting impact on you?" },
                new Thread { Id = 14, AuthorId = 17, Title = "Anime and Manga Recommendations", Content = "Explore the world of anime and manga. Share your favorite series, recommend hidden gems, and discuss the latest releases in the anime community." },
                new Thread { Id = 15, AuthorId = 19, Title = "Fitness and Gaming", Content = "How do you balance gaming with a healthy lifestyle? Share your fitness routines, tips, and experiences of staying active while enjoying your favorite games." },
                new Thread { Id = 16, AuthorId = 21, Title = "Favorite Game Soundtracks", Content = "Immerse yourself in the world of video game music. Share your favorite soundtracks and discuss the impact of music on gaming experiences." },
                new Thread { Id = 17, AuthorId = 12, Title = "Future of Virtual Reality", Content = "Predictions and discussions on the future of virtual reality. What technological advancements do you envision, and how will VR shape entertainment and beyond?" },
                new Thread { Id = 18, AuthorId = 14, Title = "Game Art Appreciation", Content = "Highlighting the artistic side of gaming. Share your favorite game art, discuss the work of talented artists, and explore the impact of visuals on gaming." },
                new Thread { Id = 19, AuthorId = 16, Title = "E-Sports Fan Zone", Content = "Cheer for your favorite E-Sports teams and discuss the latest tournaments. Share strategies, memorable moments, and predictions for the competitive gaming scene." },
                new Thread { Id = 20, AuthorId = 18, Title = "Your Gaming Achievements", Content = "Brag about your proudest gaming achievements! Whether it's completing a challenging level or reaching a high rank, share your gaming triumphs with the community." },
                new Thread { Id = 21, AuthorId = 11, Title = "Epic Boss Battles: Share Your Favorites", Content = "Discuss your most memorable boss battles in gaming history. Whether it's the intensity, strategy, or sheer epicness, let's reminisce about the epic boss fights that made our gaming experiences unforgettable!" },
                new Thread { Id = 22, AuthorId = 13, Title = "Co-op Gaming Night: Looking for Teammates", Content = "Planning a co-op gaming night and need teammates? Share the games you're playing, your preferred platform, and connect with fellow gamers for a night of cooperative gaming fun. Let's build gaming squads and forge new alliances!" },
                new Thread { Id = 23, AuthorId = 15, Title = "Hidden Gems: Underrated Games Worth Playing", Content = "Discover and share underrated or lesser-known games that deserve more attention. Whether it's an indie gem or a hidden classic, let's create a list of hidden gaming treasures for others to explore and enjoy!" },
                new Thread { Id = 24, AuthorId = 17, Title = "Gaming Challenges: Push Your Skills to the Limit", Content = "Challenge fellow gamers to try specific in-game challenges. From speed runs to completing games on the highest difficulty, share your gaming challenges and see who can conquer them. Who's up for the ultimate gaming challenge?" },
                new Thread { Id = 25, AuthorId = 19, Title = "Virtual Reality Dreams: Share Your Wishlist", Content = "If you could design the perfect virtual reality game or experience, what would it be? Share your VR dreams, from immersive worlds to innovative gameplay mechanics. Let's imagine the future of virtual reality gaming together!" },
            };
            modelBuilder.Entity<Thread>().HasData(threads);

            List<Comment> comments = new List<Comment>()
            {
                new Comment { Id = 1, AuthorId = 2, ThreadId = 3, Content = "Has anyone played the new God of War yet?" },
                new Comment { Id = 2, AuthorId = 1, ThreadId = 3, Content = "The latest update on DokeV says it will be postponed to Q3 2015"},
                new Comment { Id = 3, AuthorId = 2, ThreadId = 1, Content = "I recently finished Elden Ring and it's easily in my top three RPGs. The open-world exploration and challenging gameplay are incredible!" },
                new Comment { Id = 4, AuthorId = 4, ThreadId = 1, Content = "Final Fantasy XVI has been my go-to RPG this year. The storytelling and character development are phenomenal, and the combat system is addictive!" },
                new Comment { Id = 5, AuthorId = 6, ThreadId = 1, Content = "Cyberpunk 2077 has improved a lot since its launch. The updates brought new life to the game, and the depth of the storyline keeps me engaged." },
                new Comment { Id = 6, AuthorId = 8, ThreadId = 1, Content = "Dragon Age 4 seems promising from what I've seen so far. Bioware never disappoints when it comes to rich narratives and memorable companions." },
                new Comment { Id = 7, AuthorId = 3, ThreadId = 1, Content = "Persona 6 was my most anticipated RPG release, and it exceeded my expectations. The blend of social simulation and dungeon crawling is addictively fun." },
                new Comment { Id = 8, AuthorId = 9, ThreadId = 1, Content = "Witcher 4 has been confirmed, and I can't wait to dive back into Geralt's adventures. The Witcher series has always delivered top-notch RPG experiences." },
                new Comment { Id = 9, AuthorId = 5, ThreadId = 1, Content = "Mass Effect: Legendary Edition is a fantastic remaster. Reliving the epic journey of Commander Shepard is an absolute joy." },
                new Comment { Id = 10, AuthorId = 7, ThreadId = 1, Content = "I've been enjoying Bravely Default II lately. The classic JRPG feel mixed with modern mechanics makes it a delightful experience." },
                new Comment { Id = 11, AuthorId = 10, ThreadId = 1, Content = "The upcoming Hogwarts Legacy RPG looks promising. Exploring the wizarding world in an open-world setting sounds magical!" },
                new Comment { Id = 12, AuthorId = 1, ThreadId = 1, Content = "I can't decide between Tales of Arise and Horizon Forbidden West. Both offer unique experiences and stunning visuals that make them stand out." },
                new Comment { Id = 13, AuthorId = 4, ThreadId = 2, Content = "I recently upgraded my gaming keyboard and mouse to the latest models. Mechanical keys and customizable buttons have significantly improved my gaming experience!" },
                new Comment { Id = 14, AuthorId = 6, ThreadId = 2, Content = "My setup includes a dedicated area for console and board game nights with friends. Bean bags, snacks, and multiple controllers make it the ultimate hangout spot!" },
                new Comment { Id = 15, AuthorId = 8, ThreadId = 2, Content = "My gaming setup is all about portability. A powerful gaming laptop, wireless peripherals, and a compact desk allow me to game anywhere, anytime." },
                new Comment { Id = 16, AuthorId = 3, ThreadId = 2, Content = "I'm a fan of DIY projects. I built my gaming desk from scratch, complete with hidden compartments for cable management. It's a labor of love!" },
                new Comment { Id = 17, AuthorId = 9, ThreadId = 2, Content = "My gaming setup is themed around my favorite game. Posters, collectibles, and a custom PC case designed after my favorite in-game character. It's a tribute to my gaming passion!" },
                new Comment { Id = 18, AuthorId = 5, ThreadId = 2, Content = "I've got a multi-purpose setup for gaming and content creation. High-resolution monitors, studio-grade microphone, and powerful editing software for the best of both worlds." },
                new Comment { Id = 19, AuthorId = 7, ThreadId = 2, Content = "I'm proud of my DIY acoustic treatment in my gaming room. No more echoes during intense gaming sessions, just pure sound immersion!" },
                new Comment { Id = 20, AuthorId = 10, ThreadId = 2, Content = "Just added a dedicated VR treadmill to my setup. It takes gaming to a whole new level of realism! Who needs a gym when you have VR fitness?" },
                new Comment { Id = 21, AuthorId = 1, ThreadId = 2, Content = "I'm planning to build a gaming PC from scratch soon. Any tips or recommended build guides would be greatly appreciated!" },
                new Comment { Id = 22, AuthorId = 2, ThreadId = 2, Content = "Added some ambient lighting to my setup. It enhances the gaming atmosphere, especially during late-night gaming sessions!" },
                new Comment { Id = 23, AuthorId = 6, ThreadId = 4, Content = "One of the key aspects in game development is optimizing your code. Profiling tools help identify performance bottlenecks, allowing for smoother gameplay." },
                new Comment { Id = 24, AuthorId = 8, ThreadId = 4, Content = "Iterative design is crucial. Don't be afraid to prototype and iterate on your game mechanics. Embrace feedback and constantly refine your ideas." },
                new Comment { Id = 25, AuthorId = 3, ThreadId = 4, Content = "Maintaining a balance between creativity and feasibility is essential. Dream big, but also consider the scope and resources required for your game project." },
                new Comment { Id = 26, AuthorId = 9, ThreadId = 4, Content = "Documentation is often underrated but incredibly important. Detailed documentation aids in understanding your code and design decisions, especially for team projects." },
                new Comment { Id = 27, AuthorId = 5, ThreadId = 4, Content = "Playtesting with a diverse group is invaluable. It helps identify gameplay issues, accessibility concerns, and ensures a more inclusive gaming experience." },
                new Comment { Id = 28, AuthorId = 7, ThreadId = 4, Content = "Learning from failures is part of the process. Don't get discouraged by setbacks; instead, analyze what went wrong and use it as a learning opportunity." },
                new Comment { Id = 29, AuthorId = 10, ThreadId = 4, Content = "Networking in the game development community is essential. Engage with fellow developers, attend conferences, and participate in game jams to expand your knowledge and connections." },
                new Comment { Id = 30, AuthorId = 1, ThreadId = 4, Content = "Adopting version control early on saves headaches later. Git, SVN, or other version control systems help manage code changes and collaboration seamlessly." },
                new Comment { Id = 31, AuthorId = 2, ThreadId = 4, Content = "Consider the user experience (UX) from the beginning. Intuitive UI and engaging gameplay are critical for player retention and enjoyment." },
                new Comment { Id = 32, AuthorId = 4, ThreadId = 4, Content = "Remember to prioritize optimization without sacrificing creativity. Finding the balance between performance and visual fidelity is a continuous challenge." },
                new Comment { Id = 33, AuthorId = 7, ThreadId = 5, Content = "I recently tried 'Half-Life: Alyx' in VR, and it blew me away. The level of immersion and interactive elements in the game are mind-blowing!" },
                new Comment { Id = 34, AuthorId = 9, ThreadId = 5, Content = "I'm a fan of rhythm games, and 'Beat Saber' remains one of my all-time favorites in VR. The energetic gameplay paired with great music is addictive!" },
                new Comment { Id = 35, AuthorId = 1, ThreadId = 5, Content = "Exploring 'The Walking Dead: Saints & Sinners' in VR was an intense experience. The survival mechanics and decision-making kept me on edge throughout the game." },
                new Comment { Id = 36, AuthorId = 2, ThreadId = 5, Content = "I tried 'Superhot VR' recently, and the time-based mechanics make it a unique and thrilling VR experience. Dodging bullets in slow motion feels incredibly badass!" },
                new Comment { Id = 37, AuthorId = 4, ThreadId = 5, Content = "VRChat is more than just a game; it's a virtual social platform. Meeting people from around the world in VR and exploring user-created worlds is an adventure in itself." },
                new Comment { Id = 38, AuthorId = 6, ThreadId = 5, Content = "The sense of scale in 'Asgard's Wrath' is astonishing. Being a Norse god in VR and interacting with the world feels epic and immersive!" },
                new Comment { Id = 39, AuthorId = 8, ThreadId = 5, Content = "I tried 'Boneworks' recently, and the physics-based interactions make it one of the most realistic VR games I've experienced. The freedom of movement is impressive!" },
                new Comment { Id = 40, AuthorId = 10, ThreadId = 5, Content = "For fans of puzzle games, 'Moss' is a gem in VR. Controlling Quill and navigating through beautifully crafted environments is enchanting." },
                new Comment { Id = 41, AuthorId = 5, ThreadId = 5, Content = "I'm excited about the upcoming VR titles announced at the latest expo. VR gaming keeps getting better, and I can't wait to dive into these new experiences!" },
                new Comment { Id = 42, AuthorId = 7, ThreadId = 5, Content = "'The Elder Scrolls V: Skyrim VR' is a classic brought into VR. The vast open world and immersive gameplay make it a must-play for any Skyrim fan." },
                new Comment { Id = 43, AuthorId = 13, ThreadId = 11, Content = "Among Us has to be one of the standout multiplayer games of the decade. The social deduction aspect is pure genius and leads to hilarious moments!" },
                new Comment { Id = 44, AuthorId = 15, ThreadId = 11, Content = "Overwatch has kept me hooked with its diverse hero roster and team-based gameplay. The constant updates and events make each session feel fresh." },
                new Comment { Id = 45, AuthorId = 17, ThreadId = 11, Content = "Rocket League's unique blend of soccer and vehicular mayhem is unmatched. The simple concept delivers intense and surprisingly strategic gameplay." },
                new Comment { Id = 46, AuthorId = 19, ThreadId = 11, Content = "The co-op experience in Monster Hunter: World is fantastic. Teaming up with friends to hunt massive beasts never gets old, and the variety of weapons adds depth to the gameplay." },
                new Comment { Id = 47, AuthorId = 21, ThreadId = 11, Content = "Fortnite has become a cultural phenomenon. The live events, collaborations, and constant updates have turned it into more than just a game—it's a social platform." },
                new Comment { Id = 48, AuthorId = 12, ThreadId = 12, Content = "I'm excited about the latest advancements in AI technology. The possibilities for enhancing gaming experiences through AI-driven narratives and dynamic environments are intriguing." },
                new Comment { Id = 49, AuthorId = 14, ThreadId = 12, Content = "The latest smartphone releases have impressive gaming capabilities. It's fascinating to see how mobile devices are becoming powerful gaming platforms in their own right." },
                new Comment { Id = 50, AuthorId = 16, ThreadId = 13, Content = "One of my all-time favorite books is 'Dune' by Frank Herbert. The world-building and intricate plot make it a masterpiece of science fiction literature." },
                new Comment { Id = 51, AuthorId = 18, ThreadId = 13, Content = "I recently delved into the world of manga with 'Attack on Titan.' The intense storytelling and unexpected twists kept me on the edge of my seat. Highly recommend it!" },
                new Comment { Id = 52, AuthorId = 20, ThreadId = 13, Content = "For fans of fantasy, 'The Name of the Wind' by Patrick Rothfuss is a must-read. The rich prose and compelling characters make it a standout in the genre." },
                new Comment { Id = 53, AuthorId = 13, ThreadId = 14, Content = "If you're into psychological thrillers, 'Death Note' is a masterpiece in both anime and manga. The cat-and-mouse game between Light and L is incredibly gripping." },
                new Comment { Id = 54, AuthorId = 15, ThreadId = 14, Content = "I'm currently enjoying 'My Hero Academia.' The superhero theme, character development, and intense battles make it a top-tier anime." },
                new Comment { Id = 55, AuthorId = 17, ThreadId = 15, Content = "Finding a balance between gaming and fitness is crucial. I've incorporated gaming into my cardio routine by cycling while playing, making workouts more enjoyable." },
                new Comment { Id = 56, AuthorId = 19, ThreadId = 15, Content = "Dance Dance Revolution is my go-to for combining gaming and exercise. It's a fun way to stay active, and the rhythm gameplay keeps me motivated." },
                new Comment { Id = 57, AuthorId = 21, ThreadId = 16, Content = "The soundtrack of 'The Legend of Zelda: Breath of the Wild' is a work of art. The music perfectly complements the game's exploration and adds to the overall experience." },
                new Comment { Id = 58, AuthorId = 12, ThreadId = 16, Content = "Nier: Automata has one of the most emotionally impactful soundtracks. The way it integrates with the narrative elevates the storytelling to a whole new level." },
                new Comment { Id = 59, AuthorId = 14, ThreadId = 17, Content = "Exciting times ahead for virtual reality! I'm curious to see how haptic feedback and advancements in VR technology will enhance the immersive experience." },
                new Comment { Id = 60, AuthorId = 16, ThreadId = 17, Content = "Imagine a VR MMORPG with realistic physics and expansive worlds. The future of virtual reality could revolutionize how we experience online gaming and social interactions." },
                new Comment { Id = 61, AuthorId = 18, ThreadId = 18, Content = "Art design in video games is often underappreciated. Games like 'Journey' showcase how visuals can tell a story and evoke emotions without traditional dialogue." },
                new Comment { Id = 62, AuthorId = 20, ThreadId = 18, Content = "The concept art for 'The Witcher 3' is breathtaking. It's fascinating to see the creative process behind designing characters, landscapes, and monsters." },
                new Comment { Id = 63, AuthorId = 13, ThreadId = 19, Content = "I've been following the competitive scene of Valorant. The strategic gameplay and diverse agent abilities make it a thrilling E-Sports experience." },
                new Comment { Id = 64, AuthorId = 15, ThreadId = 19, Content = "Esports has come a long way, with dedicated leagues and massive prize pools. The level of skill and teamwork in games like Dota 2 and League of Legends is impressive." },
                new Comment { Id = 65, AuthorId = 17, ThreadId = 20, Content = "Achieving the 'Platinum Trophy' in Bloodborne was a highlight for me. The challenging boss fights and intricate level design made the journey truly rewarding." },
                new Comment { Id = 66, AuthorId = 19, ThreadId = 20, Content = "Successfully completing a 'No Death Run' in Dark Souls felt like a gaming milestone. The sense of accomplishment is what makes challenging games so satisfying." },
                new Comment { Id = 67, AuthorId = 13, ThreadId = 4, Content = "The gaming community's support for indie developers is heartening. Which indie games do you believe deserve more recognition and popularity?" },
                new Comment { Id = 68, AuthorId = 14, ThreadId = 4, Content = "I appreciate when developers release regular updates and patches for their games. It shows dedication to the player community and a commitment to improving the gaming experience." },
                new Comment { Id = 69, AuthorId = 10, ThreadId = 4, Content = "Game development is a collaborative effort. How do you manage team dynamics and ensure a smooth workflow in your game development projects?" },
                new Comment { Id = 70, AuthorId = 2, ThreadId = 4, Content = "The accessibility features in modern games have improved, allowing more players to enjoy gaming. What accessibility features do you think are crucial for a positive gaming experience?" },
                new Comment { Id = 71, AuthorId = 4, ThreadId = 4, Content = "The gaming industry's commitment to environmental sustainability is commendable. What eco-friendly practices do you think should be more widely adopted in game development?" },
                new Comment { Id = 72, AuthorId = 13, ThreadId = 3, Content = "I follow several gaming news websites to stay updated, but sometimes the best announcements come from indie developers. Any indie titles you're looking forward to?" },
                new Comment { Id = 73, AuthorId = 15, ThreadId = 5, Content = "The level of immersion in VR horror games is unparalleled. The combination of realistic environments and spatial audio can make even the bravest players feel uneasy." },
                new Comment { Id = 74, AuthorId = 17, ThreadId = 5, Content = "I recently tried 'No Man's Sky' in VR, and exploring its vast, procedurally generated universe in virtual reality is an awe-inspiring experience. Highly recommend it!" },
                new Comment { Id = 75, AuthorId = 19, ThreadId = 5, Content = "The introduction of hand tracking in VR adds a new layer of interactivity. It feels natural to reach out and grab objects in virtual spaces. What are your thoughts on this technology?" },
                new Comment { Id = 76, AuthorId = 21, ThreadId = 5, Content = "Multiplayer VR games provide a unique social experience. Meeting friends in virtual spaces and engaging in cooperative or competitive activities feels like the future of online gaming." },
                new Comment { Id = 77, AuthorId = 12, ThreadId = 5, Content = "I've been exploring educational VR apps with my kids. It's a fantastic way for them to learn while having fun. What educational VR experiences have you found beneficial?" },
                new Comment { Id = 78, AuthorId = 14, ThreadId = 5, Content = "My favorite genre in VR is the escape room games. Solving puzzles in a virtual space is incredibly immersive. Any VR genres or specific games you enjoy the most?" },
                new Comment { Id = 79, AuthorId = 16, ThreadId = 5, Content = "The development of VR fitness games has motivated me to stay active. It's a fun way to exercise without feeling like a traditional workout. Have you tried any VR fitness games?" },
                new Comment { Id = 80, AuthorId = 18, ThreadId = 5, Content = "VR concerts are a unique way to experience live music. The sense of presence and interaction with the virtual environment create a memorable concert experience. Have you attended any VR concerts?" },
                new Comment { Id = 81, AuthorId = 20, ThreadId = 5, Content = "The advancements in haptic feedback for VR controllers add a new dimension to gameplay. Feeling the virtual world through touch enhances immersion. Which VR games do you think utilize haptic feedback effectively?" },
                new Comment { Id = 82, AuthorId = 13, ThreadId = 5, Content = "I recently upgraded to a wireless VR setup, and the freedom of movement it provides is incredible. No more worrying about tripping over cables. What's your preferred VR setup?" },
                new Comment { Id = 83, AuthorId = 14, ThreadId = 2, Content = "I recently upgraded my gaming chair to one with built-in speakers and haptic feedback. It adds a new layer of immersion, especially during intense gaming sessions. What's your must-have gaming accessory?" },
                new Comment { Id = 84, AuthorId = 6, ThreadId = 2, Content = "I'm a fan of minimalist setups. A clean desk, a high-refresh-rate monitor, and a comfortable chair are my essentials for an optimal gaming experience. What's the focus of your gaming setup?" },
                new Comment { Id = 85, AuthorId = 18, ThreadId = 2, Content = "The inclusion of RGB lighting in gaming peripherals adds a vibrant and customizable touch to the setup. Do you prefer a more subdued or a colorful RGB setup for your gaming station?" },
                new Comment { Id = 86, AuthorId = 10, ThreadId = 2, Content = "I turned an old bookshelf into a dedicated gaming storage unit. It holds my game collection, consoles, and accessories neatly. How do you organize and display your gaming gear?" },
                new Comment { Id = 87, AuthorId = 12, ThreadId = 2, Content = "I'm a big fan of multi-monitor setups for productivity and gaming. The extended screen real estate enhances the gaming experience and makes multitasking easier. What's your monitor setup like?" },
                new Comment { Id = 88, AuthorId = 8, ThreadId = 2, Content = "My gaming setup includes a retro corner with classic consoles and CRT TVs. Nostalgia hits differently when you can experience the games the way they were originally played." },
                new Comment { Id = 89, AuthorId = 16, ThreadId = 2, Content = "I recently upgraded my gaming keyboard and mouse to the latest models. Mechanical keys and customizable buttons have significantly improved my gaming experience!" },
                new Comment { Id = 90, AuthorId = 20, ThreadId = 2, Content = "I'm planning to invest in a gaming projector for a larger-than-life gaming experience. Anyone here using a gaming projector, and what's your feedback on it?" },
                new Comment { Id = 91, AuthorId = 2, ThreadId = 2, Content = "Added some ambient lighting to my setup. It enhances the gaming atmosphere, especially during late-night gaming sessions!" },
                new Comment { Id = 92, AuthorId = 4, ThreadId = 2, Content = "Having a dedicated gaming space is essential. It helps create a gaming-friendly environment, and it's where I can truly immerse myself in the gaming world. How have you customized your gaming space?" },
                new Comment { Id = 93, AuthorId = 3, ThreadId = 8, Content = "The teaser for the upcoming sci-fi RPG has me intrigued. The combination of futuristic technology and a gripping narrative is something I've been craving. What upcoming games are you most excited about?" },
                new Comment { Id = 94, AuthorId = 9, ThreadId = 8, Content = "I always check out gameplay trailers and developer interviews for upcoming games. It gives a glimpse into the mechanics and design philosophy. Any specific gameplay elements you look for in upcoming releases?" },
                new Comment { Id = 95, AuthorId = 11, ThreadId = 8, Content = "The announcement of a remastered classic is always a treat. It's a chance for new players to experience a beloved game with updated visuals and improvements. What remasters are you hoping for?" },
                new Comment { Id = 96, AuthorId = 15, ThreadId = 8, Content = "I'm a fan of open-world games, and the upcoming title's expansive map has me excited. Exploring diverse landscapes and uncovering hidden secrets is one of my favorite gaming experiences. What game settings do you enjoy the most?" },
                new Comment { Id = 97, AuthorId = 7, ThreadId = 8, Content = "The trend of post-launch content and expansions keeps games fresh and engaging. Which games do you think have excelled in delivering meaningful post-launch content?" },
                new Comment { Id = 98, AuthorId = 13, ThreadId = 8, Content = "I appreciate when developers provide regular updates on the development progress. It builds anticipation and shows transparency. Are there any upcoming games with particularly transparent development teams?" },
                new Comment { Id = 99, AuthorId = 17, ThreadId = 8, Content = "The integration of player feedback during beta testing often leads to a more polished final release. Have you participated in any game betas, and what was your experience like?" },
                new Comment { Id = 100, AuthorId = 19, ThreadId = 8, Content = "I prefer to go into new game releases with minimal information, avoiding spoilers and trailers. It adds an element of surprise and discovery. How do you approach anticipating new game releases?" },
                new Comment { Id = 101, AuthorId = 1, ThreadId = 8, Content = "The upcoming title's unique art style caught my attention. It's refreshing to see games explore different visual aesthetics. Which games do you think stand out for their art direction?" },
                new Comment { Id = 102, AuthorId = 5, ThreadId = 8, Content = "The inclusion of a photo mode in games allows players to capture stunning in-game moments. What games do you think have the best photo mode features?" },
                new Comment { Id = 103, AuthorId = 6, ThreadId = 9, Content = "One of my favorite gaming quotes is from a classic RPG: 'A hero need not speak, for when he is gone, the world will speak for him.' It encapsulates the impact of a player's journey on the game world. What gaming quotes resonate with you?" },
                new Comment { Id = 104, AuthorId = 8, ThreadId = 9, Content = "The quote 'War. War never changes.' from a certain post-apocalyptic series has become iconic. It sets the tone for the series and reflects on the enduring nature of conflict. What gaming quotes have become ingrained in your memory?" },
                new Comment { Id = 105, AuthorId = 10, ThreadId = 9, Content = "The dialogue choices in games often lead to memorable quotes. 'A man chooses, a slave obeys.' from a certain narrative-driven game is both chilling and thought-provoking. Share your favorite impactful gaming quotes!" },
                new Comment { Id = 106, AuthorId = 12, ThreadId = 9, Content = "In-game banter between characters can be comedic and memorable. What are some humorous gaming quotes that still make you laugh when you recall them?" },
                new Comment { Id = 107, AuthorId = 2, ThreadId = 11, Content = "The intricate level design in 'Sekiro: Shadows Die Twice' makes every encounter challenging yet rewarding. Mastering the combat system is key to success. What are your thoughts on this game's difficulty?" },
                new Comment { Id = 108, AuthorId = 4, ThreadId = 11, Content = "I appreciate the blend of stealth and action in 'Metal Gear Solid V: The Phantom Pain.' The open-world approach and freedom in tackling missions add a dynamic layer to the game. Which Metal Gear Solid installment is your favorite?" },
                new Comment { Id = 109, AuthorId = 6, ThreadId = 11, Content = "Playing 'Dark Souls III' feels like embarking on an epic journey filled with challenging bosses and atmospheric locations. The interconnected world design adds to the sense of exploration. Share your memorable experiences from this game." },
                new Comment { Id = 110, AuthorId = 8, ThreadId = 11, Content = "The storytelling in 'The Witcher 3: Wild Hunt' is top-notch. The choices you make and the impact on the world create a truly immersive experience. Who's your favorite character in The Witcher universe?" },
                new Comment { Id = 111, AuthorId = 10, ThreadId = 11, Content = "'Assassin's Creed Odyssey' offers a vast open world to explore with a rich historical backdrop. The branching narrative and character choices add replay value. Have you tried different character builds in the game?" },
                new Comment { Id = 112, AuthorId = 12, ThreadId = 11, Content = "'Red Dead Redemption 2' is a masterpiece in terms of storytelling and world-building. The attention to detail and the moral choices in the game create a memorable experience. Share your cowboy adventures in the Wild West!" },
                new Comment { Id = 113, AuthorId = 14, ThreadId = 11, Content = "The fluid combat system in 'Nioh' sets it apart from other action RPGs. The incorporation of Japanese mythology adds a unique flavor to the game. Which weapon type do you prefer in Nioh?" },
                new Comment { Id = 114, AuthorId = 16, ThreadId = 11, Content = "'Horizon Zero Dawn' combines a captivating story with stunning visuals. The post-apocalyptic world and robotic creatures make it a standout title. What's your favorite aspect of Aloy's journey?" },
                new Comment { Id = 115, AuthorId = 18, ThreadId = 11, Content = "Exploring the mysterious island in 'Shadow of the Colossus' is a melancholic yet beautiful experience. The scale of the colossi adds a sense of grandeur to the game. Which colossus encounter left a lasting impression on you?" },
                new Comment { Id = 116, AuthorId = 1, ThreadId = 12, Content = "The narrative depth of 'Mass Effect 2' is remarkable. The loyalty missions and the impact on the suicide mission create a memorable journey. Who were your go-to squad members for the final mission?" },
                new Comment { Id = 117, AuthorId = 3, ThreadId = 12, Content = "The atmosphere and storytelling in 'Bioshock Infinite' are mind-bending. The city of Columbia and the narrative twists make it a thought-provoking experience. Share your reflections on the game's story." },
                new Comment { Id = 118, AuthorId = 5, ThreadId = 12, Content = "'The Last of Us Part II' delves into complex themes with emotionally charged storytelling. The character development and moral dilemmas add layers to the narrative. How did the game's story impact you?" },
                new Comment { Id = 119, AuthorId = 7, ThreadId = 12, Content = "The time-travel mechanics in 'Chrono Trigger' set a standard for classic RPGs. The multiple endings and character-driven narrative contribute to its timeless appeal. What's your favorite era in the game?" },
                new Comment { Id = 120, AuthorId = 9, ThreadId = 12, Content = "'Star Wars: Knights of the Old Republic' is a classic with a rich Star Wars narrative. The choices in character development and the Force alignment system add replay value. Light side or dark side playthrough?" },
                new Comment { Id = 121, AuthorId = 11, ThreadId = 12, Content = "The exploration and puzzle-solving in 'Uncharted 4: A Thief's End' create an adventurous experience. The camaraderie between Nathan and his companions adds a personal touch. Share your favorite moments from the game." },
                new Comment { Id = 122, AuthorId = 13, ThreadId = 12, Content = "'Final Fantasy VII' holds a special place in many gamers' hearts. The iconic characters, memorable music, and the emotional impact of certain events make it a timeless classic. What's your favorite Final Fantasy moment?" },
                new Comment { Id = 123, AuthorId = 15, ThreadId = 12, Content = "The world-building in 'Elder Scrolls V: Skyrim' is vast and immersive. The freedom to explore and the countless quests offer endless possibilities. Which faction or guild did you join first in Skyrim?" },
                new Comment { Id = 124, AuthorId = 17, ThreadId = 12, Content = "'Persona 5' seamlessly combines social simulation with dungeon crawling. The stylish aesthetics and the diverse cast of characters make it a standout JRPG. Who's your favorite Phantom Thief?" },
                new Comment { Id = 125, AuthorId = 19, ThreadId = 13, Content = "'Half-Life: Alyx' showcases the potential of VR gaming. The immersive environments and innovative gameplay mechanics redefine the first-person shooter genre. Share your favorite moments from Alyx's journey." },
                new Comment { Id = 126, AuthorId = 20, ThreadId = 13, Content = "The sense of scale in 'Asgard's Wrath' is astonishing. Being a Norse god in VR and interacting with the world feels epic and immersive. What other VR titles have left a lasting impression on you?" },
                new Comment { Id = 127, AuthorId = 1, ThreadId = 13, Content = "'Boneworks' is a game-changer in VR. The physics-based interactions and freedom of movement make it one of the most realistic VR experiences. How has Boneworks influenced your perception of VR gaming?" },
                new Comment { Id = 128, AuthorId = 2, ThreadId = 13, Content = "For fans of puzzle games, 'Moss' is a gem in VR. Controlling Quill and navigating through beautifully crafted environments is enchanting. Share your favorite VR puzzle games!" },
                new Comment { Id = 129, AuthorId = 3, ThreadId = 13, Content = "The upcoming VR titles announced at the latest expo have me excited. VR gaming keeps evolving, and I can't wait to explore these new experiences. Which upcoming VR game are you most looking forward to?" },
                new Comment { Id = 130, AuthorId = 4, ThreadId = 13, Content = "'The Elder Scrolls V: Skyrim VR' is a classic brought into VR. The vast open world and immersive gameplay make it a must-play for any Skyrim fan. How does the VR experience compare to the original?" },
                new Comment { Id = 131, AuthorId = 5, ThreadId = 13, Content = "'Superhot VR' is a unique and thrilling experience. The time-based mechanics make dodging bullets in slow motion feel incredibly badass. Have you tried any other innovative VR games?" },
                new Comment { Id = 132, AuthorId = 6, ThreadId = 13, Content = "I recently tried 'Beat Saber' in VR, and it remains one of my all-time favorites. The energetic gameplay paired with great music is addictive. What's your go-to rhythm game in VR?" },
                new Comment { Id = 133, AuthorId = 7, ThreadId = 14, Content = "The world of 'VRChat' goes beyond gaming; it's a virtual social platform. Meeting people from around the world in VR and exploring user-created worlds is an adventure in itself. Share your memorable experiences in VRChat!" },
                new Comment { Id = 134, AuthorId = 8, ThreadId = 14, Content = "'Firewatch' is a captivating experience with its atmospheric storytelling. The beautiful Wyoming wilderness and the emotional narrative make it a standout title. What other games do you recommend for their storytelling?" },
                new Comment { Id = 135, AuthorId = 9, ThreadId = 14, Content = "Adding ambient lighting to my gaming setup enhanced the gaming atmosphere, especially during late-night sessions. Do you have any tips for improving the ambiance in a gaming room?" },
                new Comment { Id = 136, AuthorId = 10, ThreadId = 14, Content = "DIY acoustic treatment is a game-changer in my gaming room. No more echoes during intense gaming sessions, just pure sound immersion. Have you implemented any DIY improvements to your gaming setup?" },
                new Comment { Id = 137, AuthorId = 11, ThreadId = 14, Content = "My gaming setup is themed around my favorite game. Posters, collectibles, and a custom PC case designed after my favorite character make it a tribute to my gaming passion. What's the theme of your gaming setup?" },
                new Comment { Id = 138, AuthorId = 12, ThreadId = 14, Content = "I recently upgraded my gaming keyboard and mouse to the latest models. Mechanical keys and customizable buttons have significantly improved my gaming experience. What peripherals are essential in your gaming setup?" },
                new Comment { Id = 139, AuthorId = 13, ThreadId = 14, Content = "My gaming setup includes a dedicated area for console and board game nights with friends. Bean bags, snacks, and multiple controllers make it the ultimate hangout spot. How do you create a social space in your gaming area?" },
                new Comment { Id = 140, AuthorId = 14, ThreadId = 14, Content = "My gaming setup is all about portability. A powerful gaming laptop, wireless peripherals and compact" },
                new Comment { Id = 141, AuthorId = 15, ThreadId = 15, Content = "I've got a multi-purpose setup for gaming and content creation. High-resolution monitors, a studio-grade microphone, and powerful editing software provide the best of both worlds. What tools do you find essential for content creation?" },
                new Comment { Id = 142, AuthorId = 16, ThreadId = 15, Content = "The VR treadmill is a fantastic addition to my setup. It takes gaming to a whole new level of realism, and the VR fitness benefits are a plus. Have you tried incorporating fitness into your gaming routine?" },
                new Comment { Id = 143, AuthorId = 17, ThreadId = 15, Content = "My setup includes dedicated areas for different gaming genres. It helps maintain organization and ensures a tailored experience for each gaming session. How do you organize your gaming space?" },
                new Comment { Id = 144, AuthorId = 18, ThreadId = 15, Content = "The gaming industry's recent focus on inclusivity is commendable. Diverse representation in games and the inclusion of various perspectives contribute to a more enriching gaming experience. Share your thoughts on diversity in gaming." },
                new Comment { Id = 145, AuthorId = 19, ThreadId = 15, Content = "I'm planning to build a gaming PC from scratch soon. Any tips or recommended build guides would be greatly appreciated. What components do you consider essential for a gaming PC build?" },
                new Comment { Id = 146, AuthorId = 20, ThreadId = 15, Content = "Adding a dedicated VR headset hanger to my setup has helped keep things organized. It's a small addition that makes a big difference. What organizational solutions have you implemented in your gaming area?" },
                new Comment { Id = 147, AuthorId = 1, ThreadId = 15, Content = "I recently upgraded my gaming chair to a more ergonomic model. Comfort is key for long gaming sessions. What features do you prioritize in a gaming chair?" },
                new Comment { Id = 148, AuthorId = 2, ThreadId = 15, Content = "The gaming community's creativity in designing custom PC cases is impressive. It adds a personal touch to the setup. Have you come across any unique or themed PC cases that caught your attention?" },
                new Comment { Id = 149, AuthorId = 3, ThreadId = 16, Content = "Iterative design is crucial in game development. Prototyping and iterating on game mechanics allow for continuous improvement. What iterative design practices have you found most effective?" },
                new Comment { Id = 150, AuthorId = 4, ThreadId = 16, Content = "The balance between creativity and feasibility is a constant challenge in game development. Dreaming big while considering resources is key. How do you strike a balance in your game development projects?" },
                new Comment { Id = 151, AuthorId = 5, ThreadId = 16, Content = "Documentation often gets underrated, but it's crucial in game development. Detailed documentation aids in understanding code and design decisions. How do you approach documentation in your projects?" },
                new Comment { Id = 152, AuthorId = 6, ThreadId = 16, Content = "Playtesting with a diverse group is invaluable for identifying gameplay issues and ensuring inclusivity. What playtesting practices do you follow in your game development process?" },
                new Comment { Id = 153, AuthorId = 7, ThreadId = 16, Content = "Learning from failures is an essential part of the game development process. Setbacks provide valuable lessons. How do you approach and learn from failures in your projects?" },
                new Comment { Id = 154, AuthorId = 8, ThreadId = 16, Content = "Networking in the game development community is crucial for learning and expanding connections. Conferences and game jams are excellent opportunities. What networking tips do you have for fellow game developers?" },
                new Comment { Id = 155, AuthorId = 9, ThreadId = 16, Content = "Version control is a lifesaver in game development. Adopting it early prevents headaches later on. Which version control system do you prefer for game development?" },
                new Comment { Id = 156, AuthorId = 10, ThreadId = 16, Content = "Considering user experience (UX) from the beginning is vital in game development. Intuitive UI and engaging gameplay contribute to player enjoyment. What UX principles do you prioritize in your games?" },
            };
            modelBuilder.Entity<Comment>().HasData(comments);

            List<Rating> ratings = new List<Rating>()
            {
                new Rating() { Id = 1, ThreadId = 1, UserId = 3, Value = 5 },
                new Rating() { Id = 2, ThreadId = 1, UserId = 2, Value = 2 },
                new Rating() { Id = 3, ThreadId = 2, UserId = 3, Value = 1 },
                new Rating() { Id = 4, ThreadId = 2, UserId = 2, Value = 3 },
                new Rating() { Id = 5, ThreadId = 3, UserId = 3, Value = 5 },
                new Rating() { Id = 6, ThreadId = 3, UserId = 2, Value = 5 },
                new Rating() { Id = 7, ThreadId = 4, UserId = 4, Value = 4 },
                new Rating() { Id = 8, ThreadId = 4, UserId = 6, Value = 3 },
                new Rating() { Id = 9, ThreadId = 5, UserId = 7, Value = 5 },
                new Rating() { Id = 10, ThreadId = 5, UserId = 8, Value = 4 },
                new Rating() { Id = 11, ThreadId = 6, UserId = 9, Value = 2 },
                new Rating() { Id = 12, ThreadId = 6, UserId = 10, Value = 3 },
                new Rating() { Id = 13, ThreadId = 7, UserId = 11, Value = 5 },
                new Rating() { Id = 14, ThreadId = 7, UserId = 12, Value = 4 },
                new Rating() { Id = 15, ThreadId = 8, UserId = 13, Value = 5 },
                new Rating() { Id = 16, ThreadId = 8, UserId = 14, Value = 4 },
                new Rating() { Id = 17, ThreadId = 9, UserId = 15, Value = 3 },
                new Rating() { Id = 18, ThreadId = 9, UserId = 16, Value = 5 },
                new Rating() { Id = 19, ThreadId = 10, UserId = 17, Value = 4 },
                new Rating() { Id = 20, ThreadId = 10, UserId = 18, Value = 5 },
                new Rating() { Id = 21, ThreadId = 11, UserId = 19, Value = 3 },
                new Rating() { Id = 22, ThreadId = 11, UserId = 20, Value = 4 },
                new Rating() { Id = 23, ThreadId = 12, UserId = 21, Value = 5 },
                new Rating() { Id = 24, ThreadId = 12, UserId = 1, Value = 4 },
                new Rating() { Id = 25, ThreadId = 13, UserId = 2, Value = 3 },
                new Rating() { Id = 26, ThreadId = 13, UserId = 3, Value = 5 },
                new Rating() { Id = 27, ThreadId = 14, UserId = 4, Value = 2 },
                new Rating() { Id = 28, ThreadId = 14, UserId = 5, Value = 4 },
                new Rating() { Id = 29, ThreadId = 15, UserId = 6, Value = 5 },
                new Rating() { Id = 30, ThreadId = 15, UserId = 7, Value = 3 },
                new Rating() { Id = 31, ThreadId = 16, UserId = 8, Value = 4 },
                new Rating() { Id = 32, ThreadId = 16, UserId = 9, Value = 5 },
                new Rating() { Id = 33, ThreadId = 17, UserId = 10, Value = 2 },
                new Rating() { Id = 34, ThreadId = 17, UserId = 11, Value = 4 },
                new Rating() { Id = 35, ThreadId = 18, UserId = 12, Value = 5 },
                new Rating() { Id = 36, ThreadId = 18, UserId = 13, Value = 3 },
                new Rating() { Id = 37, ThreadId = 19, UserId = 14, Value = 4 },
                new Rating() { Id = 38, ThreadId = 19, UserId = 15, Value = 5 },
                new Rating() { Id = 39, ThreadId = 20, UserId = 16, Value = 2 },
                new Rating() { Id = 40, ThreadId = 20, UserId = 17, Value = 4 }
            };

            modelBuilder.Entity<Rating>().HasData(ratings);

			// Add tags
			// Adjust fluent api for tags

			List<Tag> tags = new List<Tag>()
            {
	            new Tag() { Id = 1, Name = "News" },
	            new Tag() { Id = 2, Name = "GameDev" },
	            new Tag() { Id = 3, Name = "RPGs" },
	            new Tag() { Id = 4, Name = "Rankings" },
	            new Tag() { Id = 5, Name = "Indies" },
	            new Tag() { Id = 6, Name = "VR" },
	            new Tag() { Id = 7, Name = "Upcoming" }
            };

            modelBuilder.Entity<Tag>().HasData(tags);

			modelBuilder.Entity<Tag>()
			  .HasMany<Thread>(th => th.Threads)
			  .WithMany(t => t.Tags)
			  .UsingEntity<ThreadTag>();

			var threadTags = new List<ThreadTag>()
			{
				new ThreadTag(){ThreadId = 1, TagId = 3},
                new ThreadTag(){ThreadId = 5, TagId = 6},
				new ThreadTag(){ThreadId = 7, TagId = 5},
				new ThreadTag(){ThreadId = 8, TagId = 7}
			};
			modelBuilder.Entity<ThreadTag>().HasData(threadTags);

		}

    }
}
